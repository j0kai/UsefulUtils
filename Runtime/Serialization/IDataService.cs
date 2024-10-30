using System.Collections.Generic;

namespace UsefulUtils.Serialization {

    public interface IDataService
    {
        void Save<T>(T data, string fileName = "New Save", bool overwrite = true);
        T Load<T>(string fileName);
        void Delete(string fileName);
        void DeleteAll();
        IEnumerable<string> ListSaves();
    }

}
