namespace Blog.Web.ResultMessages
{
    public static class Messages
    {
        public static class Article{
            public static string Add(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla eklenmiştir. ";
            }

            public static string Update(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla güncellenmiştir. ";
            }

            public static string Delete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla silinmiştir. ";
            }
            public static string UndoDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makale başarıyla geri alınmştır. ";
            }
        }

        public static class Category
        {
            public static string Add(string categoryName)
            {
                return $"{categoryName} isimli kategori başarıyla eklenmiştir. ";
            }

            public static string Update(string categoryName)
            {
                return $"{categoryName} isimli kategori başarıyla güncellenmiştir. ";
            }

            public static string Delete(string categoryName)
            {
                return $"{categoryName} isimli kategori başarıyla silinmiştir. ";
            }
            public static string UndoDelete(string categoryName)
            {
                return $"{categoryName} isimli kategori başarıyla geri alınmştır. ";
            }
        }

        public static class User
        {
            public static string Add(string userName)
            {
                return $"{userName} email adresli kullanıcı başarıyla eklenmiştir. ";
            }

            public static string Update(string userName)
            {
                return $"{userName} email adresli kullanıcı başarıyla güncellenmiştir. ";
            }

            public static string Delete(string userName)
            {
                return $"{userName} email adresli kullanıcı başarıyla silinmiştir. ";
            }
        }
        public static class SocialMedia
        {
            public static string Update(string SocialType)
            {
                return $"{SocialType} bağlantınız başarıyla güncellenmiştir. ";
            }
        }
        public static class About
        {
            public static string Update(string title)
            {
                return $"{title} başlıklı hakkımda metni başarıyla güncellenmiştir. ";
            }
        }
        public static class ContactMessage
        {
            public static string Add()
            {
                return "Mesaj başarıyla iletilmiştir. ";
            }
            public static string Delete(string name)
            {
                return $" {name} isimli kişinin mesajı çöp kutusuna taşındı. ";
            }
            public static string UndoDelete(string name)
            {
                return $"{name} isimli kişinin mesajı başarıyla geri alınmştır. ";
            }
        }
    }
}
