using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//sssss
namespace LifeApp
{
    public partial class WorldSpace : Form
    {

        Graphics worldRender;
        Brush plantBrush = new SolidBrush(Color.Green);
        Brush herbivoreBrush = new SolidBrush(Color.Yellow); 
        Brush carnivoreBrush = new SolidBrush(Color.Red);

        WorldEventArgs worldsCreatures;
        public event EventHandler<WorldEventArgs> lifeIteration;

        public WorldSpace()
        {
            InitializeComponent();

            worldRender = CreateGraphics();

            Random rnd = new Random();

            lifeIteration += worldRenderer;

            List<Plant> plants = new List<Plant>();
            List<Herbivore> herbivore = new List<Herbivore>();
            List<Carnivore> carnivores = new List<Carnivore>();
            for (byte i = 0; i < 20; i++)
                {plants.Add(new Plant(new Point(rnd.Next(10, 740), rnd.Next(10, 420)), rnd.Next(40, 100), rnd.Next(1, 3))); lifeIteration += plants[i].PlantLifeIteration; }

            for (byte i = 0; i < 12; i++)
                {herbivore.Add(new Herbivore(new Point(rnd.Next(10, 740), rnd.Next(10, 420)), rnd.Next(80, 140), rnd.Next(3, 6))); lifeIteration += herbivore[i].LifeIteration; }

            for (byte i = 0; i < 8; i++)
            { carnivores.Add(new Carnivore(new Point(rnd.Next(10, 740), rnd.Next(10, 420)), rnd.Next(5, 8))); lifeIteration += carnivores[i].LifeIteration; }

            worldsCreatures = new WorldEventArgs(plants, herbivore, carnivores);
        }
        
        void worldLifeTic(WorldEventArgs e)
        {
            if (lifeIteration != null)
                lifeIteration(this, e);
        }
        void worldRenderer(object sender, WorldEventArgs e)
        {
            worldRender.Clear(Color.White);
            foreach (Plant p in e.plants)
                if (p.Alive)
                 worldRender.FillEllipse(plantBrush, new Rectangle(p.Position.X, p.Position.Y, p.GrowthProgress, p.GrowthProgress));
            foreach (Herbivore h in e.herbivores)
                if (h.Alive)
                    worldRender.FillEllipse(herbivoreBrush, h.Position.X, h.Position.Y, h.Fullness / 10, h.Fullness / 10);
            foreach (Carnivore c in e.carnivores)
                if (c.Alive)
                    worldRender.FillEllipse(carnivoreBrush, c.Position.X, c.Position.Y, c.Fullness / 10, c.Fullness / 10);

            Thread.Sleep(160);
        }

        private void BeginLife_Click(object sender, EventArgs e)
        {
            BeginLife.Dispose();
            label1.Dispose();
            while (true)
            {
                worldLifeTic(worldsCreatures);
            }
        }
    }
    public class WorldEventArgs : EventArgs
    {
        public List<Plant> plants;
        public List<Herbivore> herbivores;
        public List<Carnivore> carnivores;
        public WorldEventArgs(List<Plant> plants, List<Herbivore> herbivores, List<Carnivore> carnivores)
        {
            this.plants = plants;
            this.herbivores = herbivores;
            this.carnivores = carnivores;
        }
    }

    abstract public class Wildlife
    {
        public Point Position { get; set; }
        public bool Alive { get; set; }
        public int NutritionalValue { get; set; }
        public double GetDistanceToWildlifeObject(Wildlife obj)
        {
            return Math.Sqrt(Math.Pow((this.Position.X - obj.Position.X), 2) + Math.Pow((this.Position.Y - obj.Position.Y), 2));
        }
    }
    public class Plant : Wildlife
    {
        public int GrowthProgress { get; set; }
        public int GrowthSpeed { get; set; }
        public Plant(Point position, int nutritionalValue, int growthSpeed)
        {
            GrowthProgress = 0;
            Alive = true;
            GrowthSpeed = growthSpeed;
            this.NutritionalValue = nutritionalValue;
            this.Position = position;
        }
        public void PlantLifeIteration(object sender, WorldEventArgs e)
        {
            if (GrowthProgress < 10)
                GrowthProgress += GrowthSpeed;
        }
    }
    public class Herbivore : Wildlife
    {
        public int Fullness { get; set; }
        double MovementSpeed { get; set; }

        public Herbivore(Point position, int nutritionalValue, float movementSpeed)
        {
            Fullness = 160;
            Alive = true;
            MovementSpeed = movementSpeed;
            this.NutritionalValue = nutritionalValue;
            this.Position = position;
        }
        void MoveToRandomSide(Random rnd)
        {
            double attribute = ((double)rnd.Next(-10, 10) / 10);
            int movmentOnX = (int)(MovementSpeed * attribute);
            int movmentOnY = (int)(MovementSpeed) - Math.Abs(movmentOnX);
            if (Position.X - movmentOnX <= 740 && Position.X - movmentOnX >= 10 && Position.Y - movmentOnX <= 420 && Position.X - movmentOnX >= 10)
                Position = new Point(Position.X - movmentOnX, Position.Y - movmentOnY);
        }
        public void LifeIteration(object sender, WorldEventArgs e) //обработчик события, содержит рекацию травоядного на изменение окружающей среды
        {
            Random rnd = new Random();
            if(Alive)
            { 
                Fullness -= 5;
                if (Fullness < 100)
                {
                    Plant targetPlant = null;
                    double minDistanceToPlant = Double.MaxValue;
                    foreach (Plant p in e.plants)
                    {
                        if (p.Alive && p.GrowthProgress > 4)
                            if (GetDistanceToWildlifeObject(p) <= minDistanceToPlant)
                            {
                                minDistanceToPlant = GetDistanceToWildlifeObject(p);
                                targetPlant = p;
                            }
                    }
                    if (GetDistanceToWildlifeObject(targetPlant) < 25 && targetPlant!=null)
                    {
                        targetPlant.Alive = false; Fullness += targetPlant.NutritionalValue;
                    }
                    else if (GetDistanceToWildlifeObject(targetPlant) < 200 && targetPlant != null)
                    {

                        double attribute = ((double)Math.Abs(targetPlant.Position.X - Position.X)) / (((double)Math.Abs(targetPlant.Position.Y - Position.Y)) + ((double)Math.Abs(targetPlant.Position.X - Position.X)));
                        int movmentOnX = (int)(MovementSpeed * attribute);
                        int movmentOnY = (int)(MovementSpeed) - Math.Abs((int)(MovementSpeed * attribute));
                        if (Position.X > targetPlant.Position.X)
                            movmentOnX *= -1;
                        if (Position.Y > targetPlant.Position.Y)
                            movmentOnY *= -1;
                        Position = new Point(Position.X + movmentOnX, Position.Y + movmentOnY);
                    }
                    else
                        MoveToRandomSide(rnd);
                }
                else
                    MoveToRandomSide(rnd);
                if (Fullness <= 60)
                    Alive = false;
            }

        }
    }
    public class Carnivore : Wildlife
    {
        public int Fullness { get; set; }
        double MovementSpeed { get; set; }

        void MoveToRandomSide(Random rnd)
        {
            double attribute = ((double)rnd.Next(-10, 10) / 10);
            int movmentOnX = (int)(MovementSpeed * attribute);
            int movmentOnY = (int)(MovementSpeed) - Math.Abs(movmentOnX);
            if (Position.X - movmentOnX <= 740 && Position.X - movmentOnX >= 10 && Position.Y - movmentOnX <= 420 && Position.X - movmentOnX >= 10)
                Position = new Point(Position.X - movmentOnX, Position.Y - movmentOnY);
        }
        public void LifeIteration(object sender, WorldEventArgs e) //обработчик события, содержит рекацию плотоядного на изменение окружающей среды
        {
            Random rnd = new Random();
            if (Alive)
            {
                Fullness -= 7;
                if (Fullness < 80)
                {
                    Herbivore targetHerbivore = null;
                    double minDistanceToPlant = Double.MaxValue;
                    foreach (Herbivore p in e.herbivores)
                    {
                        if (p.Alive)
                            if (GetDistanceToWildlifeObject(p) <= minDistanceToPlant)
                            {
                                minDistanceToPlant = GetDistanceToWildlifeObject(p);
                                targetHerbivore = p;
                            }
                    }
                    if (GetDistanceToWildlifeObject(targetHerbivore) < 30 && targetHerbivore != null)
                    {
                        targetHerbivore.Alive = false; Fullness += targetHerbivore.NutritionalValue;
                    }
                    else if (GetDistanceToWildlifeObject(targetHerbivore) < 280 && targetHerbivore != null)
                    {

                        double attribute = ((double)Math.Abs(targetHerbivore.Position.X - Position.X)) / (((double)Math.Abs(targetHerbivore.Position.Y - Position.Y)) + ((double)Math.Abs(targetHerbivore.Position.X - Position.X)));
                        int movmentOnX = (int)(MovementSpeed * attribute);
                        int movmentOnY = (int)(MovementSpeed) - Math.Abs((int)(MovementSpeed * attribute));
                        if (Position.X > targetHerbivore.Position.X)
                            movmentOnX *= -1;
                        if (Position.Y > targetHerbivore.Position.Y)
                            movmentOnY *= -1;
                        Position = new Point(Position.X + movmentOnX, Position.Y + movmentOnY);
                    }
                    else
                        MoveToRandomSide(rnd);
                }
                else
                    MoveToRandomSide(rnd);
                if (Fullness <= 40)
                    Alive = false;
            }

        }
        public Carnivore(Point position, float movementSpeed)
        {
            Fullness = 140;
            Alive = true;
            MovementSpeed = movementSpeed;
            this.Position = position;
        }
    }
}
