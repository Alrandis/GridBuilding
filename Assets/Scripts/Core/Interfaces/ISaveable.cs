// Для сохранения разных систем данных как возможное расширение 
public interface ISaveable
{
    // object для того, чтобы можно было сохранять любые данные 
    object CaptureState(); 
    void RestoreState(object state);
}