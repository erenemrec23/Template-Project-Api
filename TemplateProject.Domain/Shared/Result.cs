namespace QrAssignment.Domain.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected internal Result(bool isSuccess, Error error)
        {
            // Mantıksal doğrulama: Başarılı bir işlemin hatası olamaz, başarısız bir işlemin hatası boş olamaz.
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("Başarılı bir sonuç hata içeremez.");
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Başarısız bir sonuç hata içermelidir.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        // Geriye veri dönmeyecek başarılı işlemler için (Örn: Update, Delete)
        public static Result Success() => new(true, Error.None);

        // Başarısız işlemler için
        public static Result Failure(Error error) => new(false, error);

        // Geriye veri dönecek başarılı işlemler için yardımcı metot (Aşağıdaki Generic Result<T> sınıfını üretir)
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        // Geriye veri dönmesi beklenen ama başarısız olan işlemler için
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }
}
