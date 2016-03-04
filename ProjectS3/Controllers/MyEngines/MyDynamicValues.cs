﻿using ProjectS3.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace ProjectS3.Controllers
{
    public class MyDynamicValues
    {
        ProjectS3Entities db = new ProjectS3Entities();

        public async Task setValue(string key, string value)
        {
            MyDynamicvalue info = db.MyDynamicvalue.SingleOrDefault(t => t.Key == key);

            // If dont have the key, add new key
            if (info == null)
            {
                info = new MyDynamicvalue();
                info.Key = key;
                info.value = value;
                db.MyDynamicvalue.Add(info);
                await db.SaveChangesAsync(); 
            }
            else
            {
                // set new value for key
                info.value = value;
                db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();   
            }              
        }

        public void removeValue(string key)
        {
            MyDynamicvalue info = db.MyDynamicvalue.SingleOrDefault(t => t.Key == key);

            if (info == null)
            {
                return;
            }
            else
            {
                db.MyDynamicvalue.Remove(info);
                db.SaveChanges();
            }
        }  

        public string getValue(string key)
        {
            MyDynamicvalue info = db.MyDynamicvalue.SingleOrDefault(t => t.Key == key);

            if (info == null)
            {
                return null;
            }
            else
            {
               return info.value;
            }
        }
    }
}