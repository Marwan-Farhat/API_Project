namespace Demo.Dashboard.Helpers
{
	public class PictureSettings
	{
		public static string UploadFile(IFormFile file, string folderName)
		{
			// 1. Get folder Path
			var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images", folderName);

			// 2. Set fileName unique
			var fileName = Guid.NewGuid() + file.FileName;

			// 3. Get file path
			var filePath = Path.Combine(folderPath, fileName);

			// 4. Save file as streams
			var fs = new FileStream(filePath,FileMode.Create);

			// 5. Copy file into streams
			file.CopyTo(fs);

			// 6. return fileName
			return Path.Combine("images\\products", fileName);
		}

		public static void DeleteFile (string folderName,string fileName) 
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName, fileName);

			if(File.Exists(filePath)) 
				File.Delete(filePath);
		}
	}
}
