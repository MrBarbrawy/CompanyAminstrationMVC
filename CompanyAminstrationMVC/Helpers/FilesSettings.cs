namespace CompanyAdminstrationMVC.PL.Helpers
{
    public static class FilesSettings
    {

        //Upload

        public static string UploadFile(IFormFile file , string FolderName)
        {
            //location

            //string FolderPath = $"C:\\Users\\youss\\source\\repos\\C42-G01-MVC04.Solution\\C42-G01-MVC04.PL\\wwwroot\\Files{FolderName}";

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", FolderName);

            // File Name MAKE IT UNIQUE

            string FileName = $"{Guid.NewGuid()} {file.FileName}";

            // file path

            string FilePath = Path.Combine(FolderPath, FileName);

            // Save File

          using var FileStream = new FileStream(FilePath,FileMode.Create);

            file.CopyTo(FileStream);

            return FileName;

        }




        //delete

        public static void DeleteFile(string fileName , string folderName)
        {
            string FilePath =Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\Files" , folderName ,fileName) ;

            if (File.Exists(FilePath))
             File.Delete(FilePath);

            

        }




    }
}
