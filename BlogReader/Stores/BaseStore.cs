using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;

namespace BlogReader.Stores
{
    public abstract class BaseStore
    {
        public readonly string DataItemsFolderPath;

        public BaseStore()
        {
            DataItemsFolderPath = AppDomain.CurrentDomain.BaseDirectory + @"\DataItems";
            if (Directory.Exists(DataItemsFolderPath) == false)
            {
                Directory.CreateDirectory(DataItemsFolderPath);
            }
        }

        public virtual void SaveItemsToFile<T>(string path, List<T> data)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    var serializedData = JsonConvert.SerializeObject(data);
                    sw.WriteLine(serializedData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SaveItemsToFile:" + ex);
            }
        }

        public virtual void Dispose() { }
    }
}
