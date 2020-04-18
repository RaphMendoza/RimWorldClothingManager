using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RimWorldClothingManagerLibrary
{
    public class DataProcessor
    {
        private readonly string _sourceFilePath;
        private readonly string _testXml = @"D:\Users\Raphael\Desktop\Apparel_Various.xml";

        public DataProcessor(string sourceFilePath)
        {
            _sourceFilePath = sourceFilePath;
        }

        public List<ApparelModel> LoadData()
        {
            var apparels = new List<ApparelModel>();

            var baseGameApparels = LoadDataFromBaseGame();
            apparels.AddRange(baseGameApparels);

            var moddedGameApparels = LoadDataFromMods();
            apparels.AddRange(moddedGameApparels);

            var normalizedData = NormalizeData(apparels);
            return normalizedData;
        }

        private List<ApparelModel> LoadDataFromBaseGame()
        {
            var apparels = new List<ApparelModel>();

            var baseGameFilePath = $@"{_sourceFilePath}\data";

            var filePaths = Directory.GetFiles(baseGameFilePath, "Apparel_*.xml", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                apparels.AddRange(GetApparelData(filePath));
            }

            return apparels;
        }

        private List<ApparelModel> LoadDataFromMods()
        {
            var apparels = new List<ApparelModel>();

            var modsFilePath = $@"{_sourceFilePath}\mods";

            var filePaths = Directory.GetFiles(modsFilePath, "Apparel_*.xml", SearchOption.AllDirectories)
                .Where(x => !x.Contains("1.0") && !x.Contains("Language")).ToList();

            filePaths = filePaths.Where(x => !x.Contains("Patch")).ToList();
            //TODO: Add support for changes made in mod patches

            foreach (var filePath in filePaths)
            {
                apparels.AddRange(GetApparelData(filePath));
            }

            return apparels;
        }

        private List<ApparelModel> GetApparelData(string filePath)
        {
            var xmlString = File.ReadAllText(filePath);

            var serializer = new XmlSerializer(typeof(Defs));
            using (var reader = new StringReader(xmlString))
            {
                var result = (Defs)serializer.Deserialize(reader);
                return result.ApparelModels;
            }
        }

        private List<ApparelModel> NormalizeData(List<ApparelModel> apparels)
        {
            var actualApparels = apparels.Where(x => x.Abstract == null);

            var normalizedApparel = new List<ApparelModel>();

            foreach (var actualApparel in actualApparels)
            {
                normalizedApparel.Add(UpdateApparelData(actualApparel, apparels));
            }

            return normalizedApparel;
        }

        private ApparelModel UpdateApparelData(ApparelModel actualApparel, List<ApparelModel> sourceApparels)
        {
            var parentApparels = sourceApparels.Where(x => x.Name == actualApparel.ParentName).ToList();

            if (parentApparels == null || parentApparels.Count() > 1)
            {
                throw new Exception();
            }

            var parentApparel = parentApparels.First();
            switch (string.IsNullOrWhiteSpace(parentApparel.ParentName))
            {
                case false:
                    parentApparel = UpdateApparelData(parentApparel, sourceApparels);
                    break;
            }

            //Apparel
            if (actualApparel.Apparel != null)
            {
                if (actualApparel.Apparel.BodyPartGroups == null && parentApparel.Apparel?.BodyPartGroups != null)
                {
                    actualApparel.Apparel.BodyPartGroups = parentApparel.Apparel.BodyPartGroups;
                }

                if (actualApparel.Apparel.Layers == null && parentApparel.Apparel?.Layers != null)
                {
                    actualApparel.Apparel.Layers = parentApparel.Apparel.Layers;
                }
            }
            else
            {
                actualApparel.Apparel = parentApparel.Apparel;
            }

            //ThingCategories
            if (actualApparel.ThingCategories == null && parentApparel.ThingCategories != null)
            {
                actualApparel.ThingCategories = parentApparel.ThingCategories;
            }

            //StatBases
            switch (actualApparel.StatBases == null)
            {
                case true:
                    actualApparel.StatBases = parentApparel.StatBases;
                    break;
                case false:
                    if (parentApparel.StatBases != null)
                    {
                        foreach (var propertyInfo in typeof(StatBases).GetProperties())
                        {
                            var actualApparelPropertyValue = propertyInfo.GetValue(actualApparel.StatBases);
                            var parentApparelPropertyValue = propertyInfo.GetValue(parentApparel.StatBases);

                            if ((double)actualApparelPropertyValue == 0 && actualApparelPropertyValue != parentApparelPropertyValue)
                            {
                                propertyInfo.SetValue(actualApparel.StatBases, parentApparelPropertyValue);
                            }
                        }
                    }
                    break;
            }

            //EquippedStatOffsets
            switch (actualApparel.EquippedStatOffsets == null)
            {
                case true:
                    actualApparel.EquippedStatOffsets = parentApparel.EquippedStatOffsets;
                    break;
                case false:
                    if (parentApparel.EquippedStatOffsets != null)
                    {
                        foreach (var propertyInfo in typeof(EquippedStatOffsets).GetProperties())
                        {
                            var actualApparelPropertyValue = propertyInfo.GetValue(actualApparel.EquippedStatOffsets);
                            var parentApparelPropertyValue = propertyInfo.GetValue(parentApparel.EquippedStatOffsets);

                            if ((double)actualApparelPropertyValue == 0 && actualApparelPropertyValue != parentApparelPropertyValue)
                            {
                                propertyInfo.SetValue(actualApparel.EquippedStatOffsets, parentApparelPropertyValue);
                            }
                        }
                    }
                    break;
            }

            return actualApparel;
        }
    }
}