using Android.App;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace paujo.juze.android {
  public partial class MainActivity : Activity {


    public void CreateDatabase() {
      DatabaseHelper db = new DatabaseHelper(ApplicationContext);
    }


    public void PutFlavor(Flavor newFlavor) {
      Task.Factory.StartNew(() => {
        var db = new DatabaseHelper(ApplicationContext);
        db.PutFlavor(newFlavor);
      });
    }


    public IList<Flavor> GetAllFlavors() {
      var db = new DatabaseHelper(ApplicationContext);
      return db.GetFlavors();
    }

    public IList<Nicotine> GetAllNicotines() {
      var db = new DatabaseHelper(ApplicationContext);
      return db.GetNicotines();
    }
  }
}