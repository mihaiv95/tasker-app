using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace tasker_app.DataLayer.Utils
{
    public enum UserType
    {
        AdminUser = 1,
        ExpertUser = 2,
        CompanyUser = 3
    }


    public static class MessageType
    {
        public static string Error { get { return "error"; } }
        public static string Success { get { return "success"; } }
        public static string Info { get { return "info"; } }
    }
    public enum CategoryType
    {
        Categoria1 = 1,
        Categoria2 = 2,
        Categoria3 = 3,
        Categoria4 = 4,
        AltaCategorie = 5
    }
    public enum DocumentType
    {
        Tehnic = 1,
        Financiar = 2,
        Social = 3,
        Achizitii = 4,
        ModificariContractuale = 5,
        Suplimentar = 6
    }


    public enum Subcategory
    {
        BalantaAnalitica = 1,
        BalantaGenerala = 2,
        RaportSalariat = 3,
        RegistruSalariati = 4,
        RegistruImobilizari = 5,
        ObiecteInventar = 6,
        DecontCheltuieli = 7,
        SolicitareAutorizareDecont = 8,
        SolicitareAutorizareCreditare = 9,
        RaportActivitate = 10,
        DocumenteJustificative = 11,
        ProceduraAchizitii = 12,
        FormulareAchiztii = 13,
        DosareAchizitii = 14,
        ActeConstitutive = 15,
        PlanDeAfaceri = 16,
        Buget = 17,
        ProceduraContract = 18,
        FormulareContract = 19,
        SolicitareModificari = 20,
        RegistruJurnalVanzari = 21,
        SolicitarePlatiDecont = 22,
        SolicitareRestituireCreditare = 23,
        AlteDocumenteFinanciare = 24,
        ExecutieBugetara = 25,
        Suplimentar = 26,
        NoteValidareDecont = 27,
        NoteValidareSolicitare = 28,
        NoteValidareAchizitii = 29
    }


    public enum Months
    {
        None = 0,
        Ianuarie = 1,
        Februarie = 2,
        Martie = 3,
        Aprilie = 4,
        Mai = 5,
        Iunie = 6,
        Iulie = 7,
        August = 8,
        Septembrie = 9,
        Octombrie = 10,
        Noiembrie = 11,
        Decembrie = 12
    }
    public enum ContentType
    {
        Pdf = 1,
        Excel = 2,
        Altfel = 3
    }


    public enum Years
    {
        None = 0,
        An2021 = 2,
        An2022 = 3,
        An2023 = 4
    }
    public enum AnexaType
    {
        None = 0,
        Anexa1 = 1,
        Anexa2 = 2,
        Anexa3 = 3,
        Anexa4 = 4,
        Anexa5 = 5,
        Anexa6 = 6,
        Anexa7 = 7,
        Anexa8 = 8,
        AlteDocJust = 9
    }
    public enum Judete
    {
        Alba = 1,
        Cluj = 2,
        sibiu = 3,
        Mures = 4,
        SatuMare = 5,
        Maramures = 6,
        Bihor = 7,
        Salaj = 8
    }
    public enum Zone
    {
        Urban = 1,
        Rural = 2
    }
    public enum TipuriStudii
    {
        ISCED2 = 1,
        ISCED3 = 2,
        ISCED4 = 3,
        ISCED5 = 4,
        ISCED6 = 5,
        ISCED7 = 6
    }
    public enum StatuturiAngajare
    {
        Inactiv = 1,
        Angajat = 2,
        AngajatPeContPropriu = 3
    }
    public enum SurseInformare
    {
        Xerom = 1,
        Smart = 2,
        Eveniment = 3
    }
    public enum TipCurs
    {
        Antreprenor = 1,
        Manager = 2
    }
    public enum StatusCurs
    {
        Abandon = 1,
        Certificat = 2,
        Absent = 3
    }
    public enum FormeInfiintare
    {
        SRL = 1,
        ONG = 2
    }
    public enum TipuriSubventie
    {
        eur54k = 1,
        eur69k = 2,
        eur84k = 3,
        eur99k = 4
    }
    public enum ExportExcel
    {
        ForumAccount  = 1,
        ForumNoAccount = 2,
        UserDocuments = 3,
        Documents = 4,
    }
    public enum UserDocumentType
    {
        ActePersonale = 1,
        DosarGt = 2
    }


    public enum DataGridColumnNames
    {
        CreatedAt = 1,
        FirstName = 2,
        LastName = 3,
        Sender = 4,
        Subject =5,
        SentTime = 6,
        Receivers = 7,
        Email = 8,
        DocumentName = 9,
        UploadTime=10,
        DocumentSize =11,
        UploadedByUser = 12,
        LastUpdateTime = 13,


    }


    public enum Phase2DocumentsType
    {
        Anexa1 = 1,
        Anexa2 = 2,
        AlteDocumente = 3
    }


    public enum ExpertType
    {
        None = 0,
        ExpertInformare = 1,
        ExpertImplementare = 2,
    }
}



