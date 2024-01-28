using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg.aje
{
    public interface IAJEDateTime
    {
        int ID { get; }
    }

    public abstract class AJEDateTimeBase<T> : IAJEDateTime
    {

        public int ID { get; }

        public override string ToString() {
            return ID.ToString();
        }

        protected AJEDateTimeBase(int id) {
            ID = id;

        }
    }

    public class AJEYear : AJEDateTimeBase<AJEYear>
    {
        private AJEYear(int ev) : base(ev) {
        }

        public static List<AJEYear> m_items;
        public static List<AJEYear> Items
        {
            get
            {
                if (m_items == null) {
                    m_items = new List<AJEYear>();
                    for (int i = 2016; i < DateTime.Now.Year + 2; i++) {
                        m_items.Add(new AJEYear(i));
                    }
                }
                return m_items;
            }
        }

    }

    public class AJEMonth : AJEDateTimeBase<AJEMonth>
    {
        private AJEMonth(int ev) : base(ev) {
        }

    }

    public static class BLDateTime
    {
        public static void FilterDateTimeCheckBoxes(CheckBox chb) {
            if (chb != null) {
                RepeaterItem rep = chb.BindingContainer as RepeaterItem;
                if (rep == null) return;

                AJEYear year = rep.DataItem as AJEYear;

                if (year != null) {
                    if (year.ID == DateTime.Now.Year) {
                        chb.Checked = true;
                    }
                }
                else if (rep.ItemIndex + 1 == DateTime.Now.Month) {
                    chb.Checked = true;
                }
            }
        }
    }
}