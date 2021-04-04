using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace PenzugySzovetseg.Models {
  public class ElszamolasIdoszakok {
    public int Id { get; set; }
    public int Ev { get; set; }
    public int Negyedev { get; set; }
    public bool Zarolt { get; set; }

    public override string ToString() {
      string sNegyedEv = "";

      switch (Negyedev) {
        case 1:
          sNegyedEv = "I. negyedév";
          break;
        case 2:
          sNegyedEv = "II. negyedév";
          break;
        case 3:
          sNegyedEv = "III. negyedév";
          break;
        case 4:
          sNegyedEv = "IV. negyedév";
          break;
        default:
          sNegyedEv = Negyedev.ToString();
          break;
      }
      return Ev + " - " + sNegyedEv;
    }
  }
}