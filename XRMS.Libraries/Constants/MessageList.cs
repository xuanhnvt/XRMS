using System;

namespace XRMS.Libraries.Constants
{
    public static class MessageList
    {
        public static string UserNotValid = "The User is not valid.";

        public static string PasswordNotValid = "The Password is not valid.";

        public static string UserNotAllowed = "You are not allowed to login to this application.";

        public static string SqlServerNotConnected = "Unable to connect to the server.";

        public static string ConnectionTestSucceed = "Test Succeed!";

        public static string ConnectionTestFailed = "Unable to connect to the {0}";

        public static string FieldIsEmpty = "{0} is mandatory.";

        public static string ActionSucceed = "{0} Succeed!";

        public static string ActionFailed = "{0} Failed!";

        public static string UnhandleException = "Error: ";

        public static string NoRowIsSelected = "No Row Is Selected!";

        public static string SelectedTableIsOccupied = "Table : {0} is occupied by another order. Are you sure you want to select this table?";

        public static string OrderIsLock = "Order No : {0} is locked by another user. Please try again later.";

        public static string OrderIsNotAvailable = "Order No : {0} is not available anymore.";

        public static string ProductIsNotAvailable = "{0} is not available anymore.";

        public static string ProductIsUneditable = "This row can not be {0} because it is uneditable.";

        public static string OrderIsEmpty = "Order is Empty!";

        public static string OrderCreatedSuccessfully = "This order successfully saved with the OrderNo: {0}";

        public static string OrderEditedSuccessfully = "OrderNo: {0} Edited successfully.";

        public static string NoTableIsSelected = "No table is selected!";
    }
}
