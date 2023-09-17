namespace ProjekatDiplomski.Helper
{
    public class ImageHelper
    {
        private static string wwwroot = ".\\wwwroot";
        private static string extension = ".jpg";

        public async static Task<string> SaveImage(IFormFile slika)
        {
            if (slika == null) return null;

            if (slika.Length == 0) return null;

            var filePath = Path.Combine(wwwroot, Path.ChangeExtension(Path.GetRandomFileName(), extension));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await slika.CopyToAsync(stream);
            }

            return filePath.Remove(0, 10);
        }

        public static void DeleteImage(string link)
        {
            string path = Path.Combine(wwwroot, link);

            FileInfo file = new FileInfo(path);

            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
