using AYAOPv1.Source.Abstract;
using AYAOPv1.Source.Interfaces;
using AYAOPv1.Source.MVVM.Model;
using AYAOPv1.Source.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AYAOPv1.Source.Class.Repositories
{
    public class ShortCutRepository : JsonFile, IShortCutRepository
    {
        public ShortCutRepository()
        {
            
        }

        private List<ShortCutDTO> GetShortCutsList()
        {
            try
            {
                if (string.IsNullOrEmpty(Read()))
                {
                    return new List<ShortCutDTO>();
                }
                else return JsonConvert.DeserializeObject<List<ShortCutDTO>>(Read());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void Save(ShortCutDTO shortCut)
        {
            try
            {
                var list = GetShortCutsList();
                if (list != null)
                {
                    list.Add(shortCut);
                    var json = JsonConvert.SerializeObject(list);
                    Write(json);
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ShortCutDTO Get(string name)
        {
            try
            {
                var list = GetShortCutsList();
                if (list != null)
                {
                    return list.Find(x => x.Name == name);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public IEnumerable<ShortCutDTO> GetAll()
        {
            try
            {
                var list = GetShortCutsList();
                if (list != null)
                {
                    return list;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void ChangeBgImage(string name, string bgImagePath)
        {
            try
            {
                var list = GetShortCutsList();
                if(list != null)
                {
                    var item = list.Find(x => x.Name == name);
                    item.BackgroundImagePath = bgImagePath;
                    var json = JsonConvert.SerializeObject(list);
                    Write(json);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Delete(string name)
        {
            try
            {
                var list = GetShortCutsList();
                if(list != null)
                {
                    list.Remove(list.Find(x => x.Name == name));
                    var json = JsonConvert.SerializeObject(list);
                    Write(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
