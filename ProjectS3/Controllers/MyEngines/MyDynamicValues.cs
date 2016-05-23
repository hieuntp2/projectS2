using ProjectS3.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace ProjectS3.Controllers
{
    public class MyDynamicEngine
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

        public async Task removeValue(string key)
        {
            MyDynamicvalue info = db.MyDynamicvalue.SingleOrDefault(t => t.Key == key);

            if (info == null)
            {
                return;
            }
            else
            {
                db.MyDynamicvalue.Remove(info);
                await db.SaveChangesAsync();
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

        internal async Task increaseValue(string key)
        {
            MyDynamicvalue info = db.MyDynamicvalue.SingleOrDefault(t => t.Key == key);

            // If dont have the key, add new key
            if (info == null)
            {
                info = new MyDynamicvalue();
                info.Key = key;
                info.value = "0";
                db.MyDynamicvalue.Add(info);
                await db.SaveChangesAsync();
            }
            else
            {
                // set new value for key
                info.value = (int.Parse(info.value) + 1).ToString(); ;
                db.Entry(info).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
            }    
        }
    }
}