using AYAOPv1.Source.MVVM.Model;
using System.Collections.Generic;


namespace AYAOPv1.Source.Interfaces
{
    public interface IShortCutRepository
    {
        void Save(ShortCutDTO shortCut);
        void Delete(string name);
        IEnumerable<ShortCutDTO> GetAll();
        ShortCutDTO Get(string name);
        void ChangeBgImage(string name, string bgImagePath);
    }
}
