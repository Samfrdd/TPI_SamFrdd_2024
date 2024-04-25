/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : MapData.cs est une classe Serializable qui nous permets de stocké une list de BlockData pour ensuite générer une carte entière
version 1.0
***********************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class MapData 
{
   private List<BlocData> _blocks = new List<BlocData>();

   public List<BlocData> Blocks { get => _blocks;private set => _blocks = value; }


   public void AddBlockData(BlocData bloc){
      Blocks.Add(bloc);
   }
}


