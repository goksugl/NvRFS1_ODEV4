//ODEV4

using System;
using Business;
using Core;
using DataAccess;
using Entities;

// 1. Entities (Varlıklar) Katmanı:
namespace Entities
{
    public class ProgrammingLanguage
    {
        public int Id { get; set; }  // Benzersiz kimlik
        public string Name { get; set; } // Programlama dili adı (C#, Java, Python gibi)
    }
}

namespace Entities
{
    public class Technology
    {
        public int Id { get; set; }  // Benzersiz kimlik
        public string Name { get; set; } // Teknoloji adı (ASP.NET Core, Spring gibi)
        public ProgrammingLanguage ProgrammingLanguage { get; set; } // Hangi programlama diline ait
    }
}

// 2.Data Access(Veri Erişim) Katmanı

namespace DataAccess
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T GetById(int id);
        List<T> GetAll();
    }
}

namespace DataAccess
{
    public class ProgrammingLanguageRepository : IRepository<ProgrammingLanguage>
    {
        private List<ProgrammingLanguage> _programmingLanguages = new List<ProgrammingLanguage>();

        public void Add(ProgrammingLanguage entity) => _programmingLanguages.Add(entity);

        public void Update(ProgrammingLanguage entity)
        {
            var lang = _programmingLanguages.FirstOrDefault(pl => pl.Id == entity.Id);
            if (lang != null)
            {
                lang.Name = entity.Name;
            }
        }

        public void Delete(int id) => _programmingLanguages.RemoveAll(pl => pl.Id == id);

        public ProgrammingLanguage GetById(int id) => _programmingLanguages.FirstOrDefault(pl => pl.Id == id);

        public List<ProgrammingLanguage> GetAll() => _programmingLanguages;
    }
}

namespace DataAccess
{
    public class TechnologyRepository : IRepository<Technology>
    {
        private List<Technology> _technologies = new List<Technology>();

        public void Add(Technology entity) => _technologies.Add(entity);

        public void Update(Technology entity)
        {
            var tech = _technologies.FirstOrDefault(t => t.Id == entity.Id);
            if (tech != null)
            {
                tech.Name = entity.Name;
                tech.ProgrammingLanguage = entity.ProgrammingLanguage;
            }
        }

        public void Delete(int id) => _technologies.RemoveAll(t => t.Id == id);

        public Technology GetById(int id) => _technologies.FirstOrDefault(t => t.Id == id);

        public List<Technology> GetAll() => _technologies;
    }
}

// 3. Business (İş Mantığı) Katmanı
namespace Business
{
    public class ProgrammingLanguageManager
    {
        private IRepository<ProgrammingLanguage> _repository;

        public ProgrammingLanguageManager(IRepository<ProgrammingLanguage> repository)
        {
            _repository = repository;
        }

        public void AddProgrammingLanguage(ProgrammingLanguage language)
        {
            _repository.Add(language);
        }

        public List<ProgrammingLanguage> GetAllProgrammingLanguages()
        {
            return _repository.GetAll();
        }
    }
}


// 4. Core (Yardımcı Fonksiyonlar) Katmanı

namespace Core
{
    public static class ConsoleHelper
    {
        public static void PrintLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}

// 5.Presentation(Sunum) Katmanı

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgrammingLanguageRepository repository = new ProgrammingLanguageRepository();
            ProgrammingLanguageManager manager = new ProgrammingLanguageManager(repository);

            while (true)
            {
                ConsoleHelper.PrintLine("Programlama Dilleri Yönetim Uygulaması");
                Console.WriteLine("1 - Yeni Programlama Dili Ekle");
                Console.WriteLine("2 - Tüm Programlama Dillerini Listele");
                Console.WriteLine("3 - Çıkış");

                Console.Write("Seçiminiz: ");
                int secim;
                if (!int.TryParse(Console.ReadLine(), out secim))
                {
                    Console.WriteLine("Lütfen geçerli bir sayı girin!");
                    continue;
                }

                if (secim == 1)
                {
                    Console.Write("Programlama Dili Adı: ");
                    string name = Console.ReadLine();
                    manager.AddProgrammingLanguage(new ProgrammingLanguage { Id = new Random().Next(1, 1000), Name = name });
                    ConsoleHelper.PrintLine("Programlama dili eklendi!");
                }
                else if (secim == 2)
                {
                    var languages = manager.GetAllProgrammingLanguages();
                    if (languages.Count == 0)
                    {
                        ConsoleHelper.PrintLine("Henüz programlama dili eklenmedi.");
                    }
                    else
                    {
                        ConsoleHelper.PrintLine("Programlama Dilleri:");
                        foreach (var lang in languages)
                        {
                            Console.WriteLine($"ID: {lang.Id}, Adı: {lang.Name}");
                        }
                    }
                }
                else if (secim == 3)
                {
                    ConsoleHelper.PrintLine("Çıkış yapılıyor...");
                    break;
                }
                else
                {
                    Console.WriteLine("Geçersiz seçim!");
                }
            }
        }
    }
}
