using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Org.BouncyCastle.Crypto.Digests;

namespace PenzugySzovetseg.aje {
  public class Nyomtatas {

    private readonly SqlLiteAccess m_sqlLiteAccess = new SqlLiteAccess();
    private readonly int m_Tagdij = 25;

    List<string> datumok = new List<string>() { "Január", "Február", "Március", "Április", "Május", "Június", "Július", "Augusztus", "Szeptember", "Október", "November", "December" };

    Document m_doc;
    public string Init(HttpRequest request) {
      m_doc = new Document();
      string s = "";
      string datumDir = DateTime.Now.ToString("yyyyMMdd");
      var sl = DateTime.Now.Ticks.ToString();
      if (request.PhysicalApplicationPath != null) {
        string path = string.Format(@"{0}\{1}", request.PhysicalApplicationPath, datumDir);
        if (!Directory.Exists(path)) {
          Directory.CreateDirectory(path);
        }
        s = string.Format(@"{0}\{1}.pdf", path, sl);
        PdfWriter.GetInstance(m_doc, new FileStream(s, FileMode.Create));
      }
      m_doc.Open();
      return "~/" + datumDir + "/" + sl + ".pdf";

    }

    public void PrintFizetesek(DataTable table,List<int> honapok) {
      DataTable lelkTable = m_sqlLiteAccess.LoadData("SELECT Kod,Nev FROM Lelkeszek");
      foreach (DataRow dataRow in lelkTable.Rows) {
        DataRow[] datarows = table.Select("Lelkesz='" + dataRow.ItemArray[0]+"'");
        if (datarows.Length > 0) {
          _AddPage();
          m_doc.Add(new Paragraph(" "));
          Paragraph paragraph = new Paragraph("Lelkész: " + dataRow.ItemArray[1], new Font(Font.FontFamily.TIMES_ROMAN, 22));
          //paragraph.Font = new Font(Font.FontFamily.TIMES_ROMAN, 22);
          m_doc.Add(paragraph);
          m_doc.Add(new Paragraph(" "));

          decimal dec = 0;

          PdfPTable pdfHonapok = new PdfPTable(2);

          _AddTableCell(pdfHonapok, "Elszámolt hónapok: ");
          string elszHon = "";
          for (int i = 0; i < honapok.Count; i++) {
            elszHon += datumok[honapok[i]];
            if (i < honapok.Count - 1) {
              elszHon += ", ";
            }
          }

          _AddTableCell(pdfHonapok, elszHon);

          m_doc.Add(pdfHonapok);

          foreach (DataRow row in datarows) {
            PdfPTable pdfTable = new PdfPTable(2);

            _AddTableCell(pdfTable, " ");
            _AddTableCell(pdfTable, " ");

            _AddTableCell(pdfTable, row["Varos"].ToString(), true, BaseColor.BLUE);
            _AddTableCell(pdfTable, " ");

            _AddTableCell(pdfTable, "Honorárium: ");
            _AddTableCell(pdfTable, row["Honorarium"].ToString());

            _AddTableCell(pdfTable, "Utazás: ");
            _AddTableCell(pdfTable, row["Utazas"].ToString());

            _AddTableCell(pdfTable, " ");
            _AddTableCell(pdfTable, " ");

            _AddTableCell(pdfTable, "Összesen");
            dec += (decimal)row["Osszesen"];
            _AddTableCell(pdfTable, row["Osszesen"].ToString(), true, BaseColor.DARK_GRAY);

            _AddTableCell(pdfTable, " ");
            _AddTableCell(pdfTable, " ");

            m_doc.Add(pdfTable);

          }

          PdfPTable pdfTableTeljes = new PdfPTable(2);

          _AddTableCell(pdfTableTeljes, "");
          _AddTableCell(pdfTableTeljes, "");

          _AddTableCell(pdfTableTeljes, "Teljes összeg", true, BaseColor.RED, 20);
          _AddTableCell(pdfTableTeljes, dec.ToString(), true, BaseColor.BLACK,20);

          m_doc.Add(pdfTableTeljes);

        }
      }
      m_doc.Close();
    }


    public void PrintVarosok(DataTable table, bool includeTagdij) {
      //throw new Exception("test");
        DataTable lelkTable = m_sqlLiteAccess.LoadData("SELECT Nev,TagokSzama FROM Varosok");
        foreach (DataRow dataRow in lelkTable.Rows) {
          string varosNev = dataRow.ItemArray[0].ToString();
          int tagokSzama = Convert.ToInt32(dataRow.ItemArray[1]);
          DataRow[] datarows = table.Select("Varos='" + varosNev + "'");
          if (datarows.Length > 0) {
            _AddPage();
            m_doc.Add(new Paragraph(" "));
            Paragraph paragraph = new Paragraph(varosNev + ", tagok száma: " + tagokSzama, new Font(Font.FontFamily.TIMES_ROMAN, 20));
            //paragraph.Font = new Font(Font.FontFamily.TIMES_ROMAN, 22);
            m_doc.Add(paragraph);
            m_doc.Add(new Paragraph(" "));

            decimal dec = 0;
            Font fontLeft = new Font(Font.FontFamily.TIMES_ROMAN, 14);

            foreach (DataRow row in datarows) {
              PdfPTable pdfTable = new PdfPTable(3);

              _AddTableCell(pdfTable, " ");
              _AddTableCell(pdfTable, " ");
              _AddTableCell(pdfTable, "           ");

            _AddTableCell(pdfTable, "Év: ");

            _AddTableCell(pdfTable, row["Ev"].ToString(), fontLeft, Element.ALIGN_RIGHT);
            _AddTableCell(pdfTable, "           ");

            _AddTableCell(pdfTable, "Hónap: ");

              int h = Convert.ToInt32(row["Honap"]);
              _AddTableCell(pdfTable, datumok[h - 1], fontLeft, Element.ALIGN_RIGHT);
              _AddTableCell(pdfTable, "           ");

              _AddTableCell(pdfTable, "Honorárium: ");
              _AddTableCell(pdfTable, row["Honorarium"].ToString(), fontLeft, Element.ALIGN_RIGHT);
              _AddTableCell(pdfTable, "           ");

              _AddTableCell(pdfTable, "Utazás: ");
              _AddTableCell(pdfTable, row["Utazas"].ToString(), fontLeft, Element.ALIGN_RIGHT);
              _AddTableCell(pdfTable, "           ");

              _AddTableCell(pdfTable, "Összesen");
              dec += (decimal)row["Osszesen"];
              _AddTableCell(pdfTable, row["Osszesen"].ToString(), FontFactory.GetFont(FontFactory.TIMES_BOLD, 14, BaseColor.DARK_GRAY), Element.ALIGN_RIGHT);
              _AddTableCell(pdfTable, "           ");

              m_doc.Add(pdfTable);

            }

            PdfPTable pdfTableTeljes = new PdfPTable(3);

            _AddTableCell(pdfTableTeljes, "___________________");
            _AddTableCell(pdfTableTeljes, "_________", fontLeft, Element.ALIGN_RIGHT);
            _AddTableCell(pdfTableTeljes, "           ");

            int tagdijak = 0;
            if (includeTagdij) {
              _AddTableCell(pdfTableTeljes, "Éves tagdíj: ");
              tagdijak = (tagokSzama * m_Tagdij);
              _AddTableCell(pdfTableTeljes, tagdijak.ToString(), fontLeft, Element.ALIGN_RIGHT);
              _AddTableCell(pdfTableTeljes, "           ");
            }

            _AddTableCell(pdfTableTeljes, "Teljes összeg", true, BaseColor.RED, 20);
            _AddTableCell(pdfTableTeljes, (tagdijak + dec).ToString(), FontFactory.GetFont(FontFactory.TIMES_BOLD, 20, BaseColor.BLACK), Element.ALIGN_RIGHT);
            _AddTableCell(pdfTableTeljes, "           ");

            m_doc.Add(pdfTableTeljes);

          }
        }
        m_doc.Close();

    }

    private static void _AddTableCell(PdfPTable pdfTable, string s, bool isBold = false, BaseColor fontColor = null, int fontSize = 14) {
      Font font;
      if (isBold) {
        font = FontFactory.GetFont(FontFactory.TIMES_BOLD, fontSize, fontColor);
      } else {
        font = new Font(Font.FontFamily.TIMES_ROMAN, fontSize);
      }

      _AddTableCell(pdfTable, s, font, Element.ALIGN_LEFT);


    }
    private static void _AddTableCell(PdfPTable pdfTable, string s, Font font, int element) {


      Paragraph parDetail = new Paragraph(s, font);

      PdfPCell pdfPCell = new PdfPCell(parDetail);
      pdfPCell.HorizontalAlignment = element;
      pdfPCell.Border = 0;
      pdfTable.AddCell(pdfPCell);
    }

    public void Print() {
      _AddPage();
      m_doc.Close();
    }

    private void _AddPage() {
      m_doc.NewPage();
      Image img = Image.GetInstance("http://www.szovetseg.jeneia.net/aje/logo_uj.png");
      img.ScaleAbsolute(298f, 35f);
      PdfPTable pdfTable = new PdfPTable(1);
      PdfPCell pdfPCell = new PdfPCell(img);
      pdfPCell.Border = 0;
      pdfTable.AddCell(pdfPCell);
      m_doc.Add(pdfTable);
      m_doc.Add(new Paragraph(""));
      m_doc.Add(new Paragraph(""));

      //m_doc.Add(new Paragraph("Itt lesznek adatok hamarosan...."));
    }
  }
}