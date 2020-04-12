using UnityEngine;
using Assets.Source.Components.Base;
using System.Collections.Generic;
using Assets.Source.Constants;
using System.Text;

namespace Assets.Source.Components.Enemy
{
    public class EnemyFormation : ComponentBase
    {
        public enum EnemyTypes
        {
            Simple,
            Kamikaze
        }

        public EnemyTypes enemyType;

        public Vector3 upperLeftLocation = new Vector3(-1.5f, 1.75f, -10.0f);

        [SerializeField]
        private List<List<int>> locations = new List<List<int>>(); //set any of these to 1 for an enemy to spawn in that location

        public int spawnLocationsX = 5;
        public int spawnLocationsY = 5;

        public override void ComponentAwake()
        {
            upperLeftLocation = new Vector3(-1.5f, 1.75f, -10.0f);
            InitializeLocations();
            //SetLocationRow(1);
            SetLocation(0, 0);
            SetLocation(0, 1);
            base.ComponentAwake();
        }

        public override void ComponentStart()
        {
            
            PrintFormation();

            SpawnFormation();
            base.ComponentStart();
        }

        private void InitializeLocations()
        {
            for(int x = 0; x < spawnLocationsX; x++)
            {
                List<int> row = new List<int>();
                for(int y = 0; y < spawnLocationsY; y++)
                {
                    row.Add(0);
                }
                locations.Add(row);
            }
        }

        public void UpdateLocations(List<List<int>> newLocations)
        {
            locations = newLocations;
        }

        public void SetLocation(int row, int column)
        {
            locations[row][column] = 1;
        }

        public void SetLocationRow(int row)
        {
            List<int> rowToUpdate = locations[row];
            for(int i = 0; i < rowToUpdate.Count; i++)
            {
                rowToUpdate[i] = 1;
            }
        }

        public void SetLocationColumn(int column)
        {
            foreach(List<int> row in locations)
            {
                row[column] = 1;
            }
        }

        public void SpawnFormation()
        {
            //use formation and some magic to spawn, align, number, and group enemies together
            //assume odd number
            GameObject enemyPrafabToSpawn = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/Actors/{GameObjects.Actors.ShooterEnemy}");
            SpriteRenderer spriteRenderer = enemyPrafabToSpawn.GetComponent<SpriteRenderer>();
            float width = spriteRenderer.size.x;
            float height = spriteRenderer.size.y;

            int rowIndex = 0;
            foreach(List<int> row in locations)
            {
                int columnIndex = 0;
                foreach(int i in row)
                {
                    if(i == 1)
                    {
                        float locationX = upperLeftLocation.x + columnIndex * width * 2;
                        float locationY = upperLeftLocation.y - rowIndex * height * 2;
                        Debug.Log("LocationX: " + locationX + "  upperLeft.x: " + upperLeftLocation.x + " index: " + columnIndex + " width: " + width);
                        Debug.Log("LocationY: " + locationY + "  upperLeft.y: " + upperLeftLocation.y + " index: " + columnIndex + " height: " + width);

                        GameObject enemy = InstantiatePrefab(enemyPrafabToSpawn, new Vector3(locationX, locationY, -10f));
                        Debug.Log("Spawned Enemy at: " + enemy.transform.position);
                    }
                    columnIndex++;
                }
                rowIndex++;
            }
        }

        public void PrintFormation()
        {
            StringBuilder sb = new StringBuilder();
            foreach (List<int> row in locations)
            {
                
                foreach(int column in row)
                {
                    sb.Append(column + " ");
                }
                sb.AppendLine();
            }
            //Debug.Log(sb.ToString());
        }
    }
}


