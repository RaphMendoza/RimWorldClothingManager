using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            var actualApparels = apparels.Where(x => x.Abstract == null).ToList();

            var normalizedApparel = new List<ApparelModel>();

            foreach (var actualApparel in actualApparels)
            {
                normalizedApparel.Add(MergeModel(actualApparel, apparels));
            }

            return normalizedApparel;
        }

        private ApparelModel MergeModel(ApparelModel actualApparel, List<ApparelModel> sourceApparels)
        {
            var sourceApparel = sourceApparels.FirstOrDefault(x => x.Name == actualApparel.ParentName);

            if (sourceApparel.ParentName != null)
            {
                sourceApparel = MergeModel(sourceApparel, sourceApparels);
            }

            //Apparel
            if (sourceApparel.Apparel != null)
            {
                if (actualApparel.Apparel != null)
                {
                    if (actualApparel.Apparel.BodyPartGroups != null && sourceApparel.Apparel.BodyPartGroups != null)
                    {
                        actualApparel.Apparel.BodyPartGroups =
                            actualApparel.Apparel.BodyPartGroups.Union(sourceApparel.Apparel.BodyPartGroups).ToList();
                    }
                    else
                    {
                        actualApparel.Apparel.BodyPartGroups = sourceApparel.Apparel.BodyPartGroups;
                    }

                    if (actualApparel.Apparel.Layers != null)
                    {
                        actualApparel.Apparel.Layers =
                            actualApparel.Apparel.Layers.Union(sourceApparel.Apparel.Layers).ToList();
                    }
                    else
                    {
                        actualApparel.Apparel.Layers = sourceApparel.Apparel.Layers;
                    }
                }
                else
                {
                    actualApparel.Apparel = sourceApparel.Apparel;
                } 
            }

            //ThingCategories
            if (sourceApparel.ThingCategories != null)
            {
                actualApparel.ThingCategories = actualApparel.ThingCategories.Union(sourceApparel.ThingCategories).ToList();
            }

            //StatBases
            if (sourceApparel.StatBases != null)
            {
                if (actualApparel.StatBases != null)
                {
                    foreach (var property in typeof(StatBases).GetProperties())
                    {
                        if ((double)property.GetValue(actualApparel.StatBases) == 0)
                        {
                            property.SetValue(actualApparel.StatBases, property.GetValue(sourceApparel.StatBases));
                        }
                    }
                }
                else
                {
                    actualApparel.StatBases = sourceApparel.StatBases;
                } 
            }

            //EquippedStatOffsets
            if (sourceApparel.EquippedStatOffsets != null)
            {
                if (actualApparel.EquippedStatOffsets != null)
                {
                    foreach (var property in typeof(EquippedStatOffsets).GetProperties())
                    {
                        if ((double)property.GetValue(actualApparel.EquippedStatOffsets) == 0)
                        {
                            property.SetValue(actualApparel.EquippedStatOffsets, property.GetValue(sourceApparel.EquippedStatOffsets));
                        }
                    }
                }
                else
                {
                    actualApparel.EquippedStatOffsets = sourceApparel.EquippedStatOffsets;
                } 
            }

            return actualApparel;
        }
    }
}