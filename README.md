# Firebase-database-xamarin-helpers
A serie of libraries to be more pleasurable and easy using the Xamarin Firebase iOS Nuget for the real time Database

# Usage

The models (objects) to be used as firebase objects must be inherit of the Firebase.DB.Helpers.Models.FirebaseBaseObjectModel
   
   ```
   public class Mensaje : FirebaseBaseObjectModel
   {
   }
   ```

Then it's posible convert the object to NSDictionary with the extension method
   ```
   using Firebase.DB.Helpers;

   var mock = mensaje.ToNSDictionary();
   ```

Also when the firebase Dictionary is obtained, it can be translated back to the orgin object
   ```
   var bar = mock.ToFirebaseBaseModel<models.Mensaje>();
   ```