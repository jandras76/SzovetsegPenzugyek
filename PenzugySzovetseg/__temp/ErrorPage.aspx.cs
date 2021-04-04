using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg.aje {
  public partial class AJELogin : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void LoginButton_Click(object sender, EventArgs e) {
      // Three valid username/password pairs: Scott/password, Jisun/password, and Sam/password.
      string[] users = { "szovetseg", "baden", "basel", "bern", "luzern", "stgallen", "zurich" , "mtunde"};
      string[] passwords = { "Sz0vets3g$!4512", "b@d3n36$34", "b@s3l35$3", "b3rnpe453$1", "l8z3rn6$42", "stg@ll3n$2432", "z8r1ch983$2", "m8u7!4d$3" };
      for (int i = 0; i < users.Length; i++) {
        bool validUsername = (string.Compare(UserName.Text, users[i], true) == 0);
        bool validPassword = (string.Compare(Password.Text, passwords[i], false) == 0);
        if (validUsername && validPassword) {
          FormsAuthentication.SetAuthCookie(users[i], true);

          //HttpCookie myCookie = new HttpCookie("Szov_Felh");
          //myCookie.Value = users[i];
          //Response.Cookies.Add(myCookie);
          //Session[users[i]] = users[i];
          Response.Redirect("~/Default.aspx");

          //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          //    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
          //{
          //    return Redirect(returnUrl);
          //}
          //else
          //{
          //    return RedirectToAction("Index", "Home");
          //}
          //Response.Redirect("~/aje/Wait.aspx");
          //Server.Transfer("~/Default.aspx");
        }
      }
      // If we reach here, the user's credentials were invalid
      InvalidCredentialsMessage.Visible = true;
    }

  }
}