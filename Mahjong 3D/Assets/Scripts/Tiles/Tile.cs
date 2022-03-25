using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public string tileName;
    public int tileNumber;
    public class HonorTile : Tile
    {
        protected HonorTile()
        {
        }
    }

    public class SuitedTile : Tile
    {
        public int suitRank;
        public bool isRedFive = false;
        public bool isTerminal = false;
        
        protected SuitedTile()
        {
        }
    }

    public class Circle : SuitedTile
    {
        public Circle(int suitRank, int tileNumber)
        {
            this.suitRank = suitRank;
            this.tileNumber = tileNumber;
            tileName = $"Circle{suitRank}";

            if (suitRank == 1 || suitRank == 9) isTerminal = true;
            if (suitRank == 5 && tileNumber == 1) isRedFive = true;
        }
    }

    public class Bamboo : SuitedTile
    {
        public bool isGreen = true;
        public Bamboo(int suitRank, int tileNumber)
        {
            this.suitRank = suitRank;
            this.tileNumber = tileNumber;
            tileName = $"Bamboo{suitRank}";

            if (suitRank == 1 || suitRank == 9) isTerminal = true;
            if (suitRank == 5 && tileNumber == 1) isRedFive = true;
            if (suitRank == 1 || suitRank == 5 || suitRank == 7 || suitRank == 9) isGreen = false;
        }
    }
    
    public class Man : SuitedTile
    {
        public Man(int suitRank, int tileNumber)
        {
            this.suitRank = suitRank;
            this.tileNumber = tileNumber;
            tileName = $"Man{suitRank}";

            if (suitRank == 1 || suitRank == 9) isTerminal = true;
            if (suitRank == 5 && tileNumber == 1) isRedFive = true;
        }
    }
    
    public class Dragon : HonorTile
    {
        public char dragonType;
        
        public Dragon(char dragonType, int tileNumber)
        {
            tileName = $"Dragon{dragonType}";
            this.tileNumber = tileNumber;
            this.dragonType = dragonType;
        }
    }

    public class Wind : HonorTile
    {
        public char windType;

        public Wind(char windType, int tileNumber)
        {
            tileName = $"Wind{windType}";
            this.tileNumber = tileNumber;
            this.windType = windType;
        }
    }
}
