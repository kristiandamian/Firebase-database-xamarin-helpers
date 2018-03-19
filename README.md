# Firebase-database-xamarin-helpers
A serie of libraries to be more pleasurable and easy using the Xamarin Firebase iOS Nuget for the real time Database

# Usage

The models (objects) to be used as firebase objects must be inherit of the Firebase.DB.Helpers.Models.FirebaseBaseObjectModel
   
   ```cs
   public class Mensaje : FirebaseBaseObjectModel
   {
   }
   ```

Then it's posible convert the object to NSDictionary with the extension method
   ```cs
   using Firebase.DB.Helpers;

   var foo = mensaje.ToNSDictionary();
   ```

Also when the firebase Dictionary is obtained, it can be translated back to the original object
   ```cs
   var bar = mock.ToFirebaseBaseModel<models.Mensaje>();
   ```

If any property don't need to be part of the Firebase schema, but you wanna use it in the same object just decorate it with the 'optional' attribute
   ```cs
    [Optional]
    public string ChatID { get; set; }     // this does't appear in the NSDictionary 
    [Optional]
	public string MensajeID { get; set; }  // this neither
    //-----------------------------------------
	public bool Leido { get; set; }        // this one will appear in the NSDictionary
   ```
   
   In the case of the datetime values the ticks are saved in the firebase backend and when is retrived is converted to DateTime againg
