using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.Utils
{
    public class Message
    {
        public string Text { get; set; }
        public string MessageType { get; set; }
    }

    public static class Messages
    {
        public static Message Message_LoggedInError = new Message { Text = "Utilizatorul nu a putut fi logat. Verificati informatiile introduse.", MessageType = MessageType.Error };
        public static Message Message_LoggedInSuccessfully = new Message { Text = "Autentificare s-a facut cu succes.", MessageType = MessageType.Success };
        public static Message Message_RefreshedTokenSuccess = new Message { Text = "Token-ul a fost actualizat.", MessageType = MessageType.Success };
        public static Message Message_RefreshedTokenError = new Message { Text = "Token-ul nu a putut fii actualizat.", MessageType = MessageType.Error };
        public static Message Message_RevokeTokenSuccess = new Message { Text = "Token-ul a fost revocat.", MessageType = MessageType.Success };
        public static Message Message_RevokeTokenError = new Message { Text = "Token-ul nu a putut fii revocat.", MessageType = MessageType.Error };

        public static Message Message_ValidateResetTokenSuccess = new Message { Text = "S-a validat token-ul cu succes.", MessageType = MessageType.Success };
        public static Message Message_ValidateResetTokenError = new Message { Text = "Token-ul nu a putut fi validat.", MessageType = MessageType.Error };

        public static Message Message_UserRegisteredSuccess = new Message { Text = "Utilizatorul a fost inregistrat cu succes.", MessageType = MessageType.Success };
        public static Message Message_UserRegisterError = new Message { Text = "Utilizatorul nu a putut fi inregistrat.", MessageType = MessageType.Error };
        public static Message Message_UserUpdateSuccess = new Message { Text = "Au fost schimbate datele utilizatorului.", MessageType = MessageType.Success };
        public static Message Message_UserUpdateError = new Message { Text = "Datele utilizatorului nu au putut fi schimbate.", MessageType = MessageType.Error };
        public static Message Message_UserDeleteSuccess = new Message { Text = "Utilizatorul a fost sters.", MessageType = MessageType.Success };
        public static Message Message_UserDeleteError = new Message { Text = "Utilizatorul nu a putut fi sters.", MessageType = MessageType.Error };
        
        
        public static Message Message_ApproveGrantSuccess = new Message { Text = "A fost aprobat grant-ul utilizatorului.", MessageType = MessageType.Success };
        public static Message Message_ApproveGrantError = new Message { Text = "Grant-ul nu a putut fi aprobat.", MessageType = MessageType.Error };
        

        public static Message Message_EmailAlreadyUsed = new Message { Text = "Acest email este deja folosit.", MessageType = MessageType.Info };
        

        public static Message Message_EmailVerified = new Message { Text = "Email verificat cu succes.", MessageType = MessageType.Success };
        public static Message Message_EmailVerifyError = new Message { Text = "Email-ul nu a putut fi verificat.", MessageType = MessageType.Error };

        public static Message Message_ForgottenPasswordEmailSent = new Message { Text = "V-a fost trimis un email pentru modificarea parolei dumneavoastra.", MessageType = MessageType.Success };
        public static Message Message_ForgottenPasswordEmailNotSent = new Message { Text = "Email-ul pentru modificarea parolei nu a putut fi trimis.", MessageType = MessageType.Error };
        public static Message Message_ResetPasswordSuccess = new Message { Text = "Parola a fost schimbata cu succes.", MessageType = MessageType.Success };
        public static Message Message_ResetPasswordError = new Message { Text = "Nu s-a putut schimba parola.", MessageType = MessageType.Error };


        public static Message Message_UserDataLoadError = new Message { Text = "Datele nu au putut fi incarcate.", MessageType = MessageType.Error };
        public static Message Message_UsersLoadError = new Message { Text = "Utilizatorii nu au putut fi incarcati.", MessageType = MessageType.Error };
        public static Message Message_UserNotFound = new Message { Text = "Utilizatorul nu a fost gasit.", MessageType = MessageType.Info };
        public static Message Message_SelectUsers = new Message { Text = "Pentru a face aceasta actiune trebuie sa selectati niste utilizatori.", MessageType = MessageType.Info };


        public static Message Message_DocumentsLoadError = new Message { Text = "Documentele nu au putut fi afisate.", MessageType = MessageType.Error };
        public static Message Message_DocumentsUploadError = new Message { Text = "Documentele nu au putut fi incarcate.", MessageType = MessageType.Error };
        
        public static Message Message_DocumentDeleteSuccess = new Message { Text = "Documentul a fost sters.", MessageType = MessageType.Success };
        public static Message Message_DocumentDeleteError = new Message { Text = "Documentul nu a putut fi sters.", MessageType = MessageType.Error };
        public static Message Message_DocumentTypeRequired = new Message { Text = "Selectati tipul documentelor.", MessageType = MessageType.Info };


        public static Message Message_DocumentDownloadError = new Message { Text = "Eroare la descarcarea documentului.", MessageType = MessageType.Error };
        public static Message Message_DocumentNotFound = new Message { Text = "Documentul selectat nu a fost gasit.", MessageType = MessageType.Info };


        public static Message Message_CreateSeriesError = new Message { Text = "Seria nu a putut fi adaugata.", MessageType = MessageType.Error };
        public static Message Message_CreateSeriesSuccess = new Message { Text = "Seria a fost adaugata cu succes.", MessageType = MessageType.Success };
        public static Message Message_GetSeriesError = new Message { Text = "Seriile de curs nu au putut fi afisate.", MessageType = MessageType.Error };

        public static Message Message_RemoveUserFromSeriesSuccess = new Message { Text = "Utilizatorii au fost eliminati din serie.", MessageType = MessageType.Success };
        public static Message Message_RemoveUserFromSeriesError = new Message { Text = "Utilizatorii nu au putut fi eliminati din serie.", MessageType = MessageType.Error };

        public static Message Message_AddUserToSeriesSuccess = new Message { Text = "Utilizatorii au fost adaugati in seria de curs.", MessageType = MessageType.Success };
        public static Message Message_AddUserToSeriesError = new Message { Text = "Utilizatorii nu a putut fi adaugati in seria de curs.", MessageType = MessageType.Error };

        public static Message Message_SeriesNameRequired = new Message { Text = "Dati un nume seriei.", MessageType = MessageType.Info };
        public static Message Message_SeriesNotFound = new Message { Text = "Seria de curs nu a fost gasita.", MessageType = MessageType.Info };

        public static Message Message_CommunicationDeletedSuccess = new Message { Text = "Comunicarea a fost stearsa.", MessageType = MessageType.Success };
        public static Message Message_CommunicationDeletedError = new Message { Text = "Comunicarea nu a putut fi stearsa.", MessageType = MessageType.Error };

        public static Message Message_CommunicationSentSuccess = new Message { Text = "Comunicarea a fost trimisa.", MessageType = MessageType.Success };
        public static Message Message_CommunicationSentError = new Message { Text = "Comunicarea nu a putut fi trimisa.", MessageType = MessageType.Error };
        public static Message Message_SelectReceivers = new Message { Text = "Selectati destinatarii.", MessageType = MessageType.Info };
        public static Message Message_SubjectMessageRequired = new Message { Text = "Introduceti un subiect si un mesaj.", MessageType = MessageType.Info };

        public static Message Message_CommunicationsGetError = new Message { Text = "Comunicarile nu au putut fi incarcate.", MessageType = MessageType.Error };
        public static Message Message_ContactsLoadError = new Message { Text = "Contactele nu au putut fi incarcate.", MessageType = MessageType.Error };

        public static Message Message_MesajGenericTest = new Message { Text = "Acesta este un mesaj generic.", MessageType = MessageType.Info };
        public static Message Message_MesajGenericTestError = new Message { Text = "Acesta este un mesaj generic de eroare.", MessageType = MessageType.Error };
        
        
        public static Message Message_GenerateDocumentError = new Message { Text = "Nu s-a putut genera documentul.", MessageType = MessageType.Error };
        public static Message Message_ExpertPerformingNotFound = new Message { Text = "Expertul care face actiunea nu a fost gasit.", MessageType = MessageType.Error };
    
    
        
        
        public static Message Message_ErrorUploadingEmptyFile = new Message { Text = "Nu puteti incarca documente goale.", MessageType = MessageType.Info };

        
    }
}
