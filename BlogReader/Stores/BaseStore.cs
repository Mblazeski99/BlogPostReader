using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

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
                Log.Error(ex, "SaveItemsToFile failed!");
            }
        }

        public virtual void Dispose() { }
    }
}
