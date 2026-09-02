using QrAssignment.Application.Converters;
using System.Text.Json.Serialization;

namespace QrAssignment.Application.DTOs.List
{
    // 1. En Üst Kapsayıcı Sınıf
    public class PageRequestFilterBaseDto
    {
        public DynamicQueryDto? DynamicFilterAndSort { get; set; } // Sıralama ve Filtreleme detayları

        public GlobalSearchDto? GlobalSearch { get; set; }
    }

    public class PageRequestBaseDto : PageRequestFilterBaseDto
    {
        public int PageIndex { get; set; } = 0; // Sayfa numarası (0 veya 1'den başlatmak sana kalmış)
        public int PageSize { get; set; } = 10; // Sayfa başına kayıt 

    }

    public class GlobalSearchDto
    {
        public List<string> Fields { get; set; } = new(); // Hangi kolonlarda aranacak?
        public string Value { get; set; } = string.Empty; // Aranan kelime ne?
    }

    // 2. Dinamik Sorgu Gövdesi
    public class DynamicQueryDto
    {
        public IEnumerable<DynamicQuerySortDto>? Sort { get; set; }
        public DynamicQueryFilterDto? Filter { get; set; }
    }

    // 3. Kolon Bazlı Sıralama (Birden fazla kolona göre sıralama yapılabilir)
    public class DynamicQuerySortDto
    {
        public string Field { get; set; } // Kolon adı (Örn: "Name", "CreatedDate")
        public string Dir { get; set; }   // Yön: "asc" veya "desc"

        public DynamicQuerySortDto() { }
        public DynamicQuerySortDto(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }
    }

    // 4. Kolon Bazlı Dinamik Filtre (Kendi içinde Recursive/Özyineli çalışır)
    public class DynamicQueryFilterDto
    {
        public string? Field { get; set; }      // nullable yapıldı
        public string? Operator { get; set; }   // nullable yapıldı

        [JsonConverter(typeof(FlexibleStringConverter))]
        public string? Value { get; set; }

        // "between" operatörü için aralığın bitiş değeri (frontend: value2)
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string? Value2 { get; set; }
        public string? Logic { get; set; }
        public IEnumerable<DynamicQueryFilterDto>? Filters { get; set; }

        public DynamicQueryFilterDto() { }
    }

    public class Paginate<T>
    {
        public IList<T> Items { get; set; } // Asıl verilerin olduğu liste 

        public int Index { get; set; }  // Mevcut sayfa numarası

        private int? _pageSize;
        public int? PageSize
        {
            get { return _pageSize ?? 10; }
            set { _pageSize = value; }
        }

        public int TotalFilteredItemCount { get; set; }  // Filtre sonrası kalan kayıt sayısı
        public int TotalItemCount { get; set; }          // Veritabanındaki filtresiz toplam kayıt sayısı

        // DÜZELTİLDİ: Filtrelenmiş kayıt sayısını, sayfa boyutuna bölüyoruz
        public int TotalPages => (int)Math.Ceiling(TotalFilteredItemCount / (double)(PageSize ?? 10));

        // Frontend'deki "Önceki/Sonraki" butonlarını disable/enable yapmak için 
        public bool HasPrevious => Index > 0;
        public bool HasNext => Index + 1 < TotalPages;

        public Paginate()
        {
            Items = Array.Empty<T>();
        }
    }
}