using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using static System.Console;


namespace BakeMyWorld.ConsoleManager
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

            var token = HandleLogin();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var nickname = tokenS.Claims.FirstOrDefault(x => x.Type == "Nickname")?.Value;
            var email = tokenS.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;
            var isAdministrator = tokenS.Claims.Any(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && x.Value == "Administrator");


            WriteLine($"\n\tWelcome, {nickname}, {email}");

            if (isAdministrator)
                WriteLine("\n\tYou logged in as an Administrator");
            else
                WriteLine("\n\tYou logged in as a User");

            Thread.Sleep(5000);
            Clear();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            CursorVisible = false;
            bool applicationRunning = true;

            do
            {
                WriteLine(
                     @"" +
                     "\n\t1. Cake Categories" +
                     "\n\t2. Cakes" +
                     "\n\t3. Corporate" +
                     "\n\t4. Exit");

                ConsoleKeyInfo input = ReadKey(true);

                Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        ShowCategoriesMenu(isAdministrator);
                        break;

                    case ConsoleKey.D2:
                        ShowCakesMenu(isAdministrator);
                        break;

                    case ConsoleKey.D3:
                        ShowCorporatesMenu(isAdministrator);
                        break;

                    case ConsoleKey.D4:
                        applicationRunning = false;
                        break;
                }

            } while (applicationRunning);
        }


        private static string HandleLogin()
        {
            // Declare credentials "validator"
            bool loginOk = false;
            string token = null;

            // Prompt user to enter credentials until valid
            // Open "login loop"
            while (!loginOk)
            {
                // Prompt for username
                SetCursorPosition(2, 2);
                string promptUsername = "Username: ";
                Write(promptUsername + "\n");

                // Prompt for password
                SetCursorPosition(2, 3);
                string promptPassword = "Password: ";
                Write(promptPassword);


                // Set cursor position to each prompt & Retrieve respective user credentials
                CursorVisible = true;
                SetCursorPosition(promptUsername.Length + 2, 2);
                string inputUsername = ReadLine();
                SetCursorPosition(promptPassword.Length + 2, 3);
                string inputPassword = ReadLine();


                // Check if input credentials match target credentials
                token = Authorize(inputUsername, inputPassword);
                loginOk = token != null ? true : false;

            } // the end of the "login loop"

            return token;
        }
        private static string Authorize(string inputUsername, string inputPassword)
        {
            // Instantiate new Admin object based on retrieved values
            var user = new Credentials(inputUsername, inputPassword);

            // Set TypeNameHandling to auto
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;

            // Serialize to JSON
            var httpContent = JsonConvert.SerializeObject(user, settings);

            // Construct a content object to send the data
            var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
            var byteContent = new ByteArrayContent(buffer);

            // Set the content type to JSON 
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send post request
            var response = httpClient.PostAsync("login?api-version=1", byteContent).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var tokenResponse = JsonConvert.DeserializeObject<Token>(json);
                string token = tokenResponse.Value;
                return token;
            }
            else
            {
                WriteLine("\n  Access denied");
                Thread.Sleep(2000);
                Clear();
                return null;
            }
        }


        private static void ShowCategoriesMenu(bool isAdministrator)
        {
            bool applicationRunning = true;

            do
            {
                if (isAdministrator)
                {
                    WriteLine(
                               @"" +
                               "\n\t1. Register New Category" +
                               "\n\t2. List Cake Categories" +
                               "\n\t3. Edit Cake Category" +
                               "\n\t4. Delete Cake Category" +
                               "\n\t5. Back");

                    ConsoleKeyInfo input = ReadKey(true);
                    Clear();

                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            RegisterNewCategoryView();
                            break;

                        case ConsoleKey.D2:
                            ListCakeCategories();
                            EscapeToPreviousMenu();
                            break;

                        case ConsoleKey.D3:
                            EditCakeCategory();
                            break;

                        case ConsoleKey.D4:
                            DeleteCakeCategory();
                            break;

                        case ConsoleKey.D5:
                            applicationRunning = false;
                            break;
                    }
                }

                else
                {
                    WriteLine(
                               @"" +
                               "\n\t1. List Cake Categories" +
                               "\n\t2. Back");

                    ConsoleKeyInfo input = ReadKey(true);
                    Clear();

                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            ListCakeCategories();
                            EscapeToPreviousMenu();
                            break;

                        case ConsoleKey.D2:
                            applicationRunning = false;
                            break;
                    }
                }

            } while (applicationRunning);
        }
        private static void RegisterNewCategoryView()
        {
            // Open prompt loop
            do
            {
                // Prompt
                SetCursorPosition(2, 2);
                Write("Category Name: ");

                SetCursorPosition(2, 3);
                Write("Image Url: ");


                // Set cursor to visible
                CursorVisible = true;

                // Set cursor position to each prompt subsequently & retrieve respecitve information
                int unifiedIdentation = "Category Name: ".Length + 3;

                SetCursorPosition(unifiedIdentation, 2);
                string name = ReadLine();

                SetCursorPosition(unifiedIdentation, 3);
                string imageUrlString = ReadLine();
                bool imageUrlOk = Uri.TryCreate(imageUrlString, UriKind.Absolute, out Uri imageUrlVerified)
                                    && (imageUrlVerified.Scheme == Uri.UriSchemeHttp || imageUrlVerified.Scheme == Uri.UriSchemeHttps);

                Uri imageUrl = imageUrlOk ? imageUrlVerified : new Uri("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");


                // Further confirmation request
                ConsoleKeyInfo confirmation = RequestConfirmation();

                // Respond to confirmation choice: "Yes"
                if (confirmation.Key == ConsoleKey.Y)
                {
                    // Instantiate new Category object based on retrieved values
                    var category = new Category(name, imageUrl);

                    // Set TypeNameHandling to auto
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.TypeNameHandling = TypeNameHandling.Auto;

                    // Serialize to JSON
                    var httpContent = JsonConvert.SerializeObject(category, settings);

                    // Construct a content object to send the data
                    var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
                    var byteContent = new ByteArrayContent(buffer);

                    // Set the content type to JSON 
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    // Send post request
                    var response = httpClient.PostAsync("categories?api-version=1", byteContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        WriteLine("\n  Category Added");
                    }
                    else
                    {
                        WriteLine("\n  Something went wrong...");
                    }

                    Thread.Sleep(2000);
                    Clear();
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                    Clear();

                    // Go back to prompt menu
                }

            } while (true);
        }
        private static void ListCakeCategories()
        {
            // HTTP GET https://localhost:44378/api/categories
            var response = httpClient.GetAsync("categories?api-version=1")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);

                WriteLine("\n\tId\t\tName");
                WriteLine("-------------------------------------------------------------");

                foreach (var category in categories)
                {
                    WriteLine($"\t{category.Id}\t\t{category.Name}");
                }
            }
            else
            {
                WriteLine("\n\tSomething went wrong...");
            }
        }
        private static void EditCakeCategory()
        {
            ListCakeCategories();

            Write("\n\tID: ");
            bool idValid = Int32.TryParse(ReadLine(), out int idParsed);
            int id = idValid ? idParsed : 0;

            var responseLocalizedCategory = httpClient.GetAsync($"categories/{id.ToString()}?api-version=1")
                .GetAwaiter()
                .GetResult();

            Clear();

            if (responseLocalizedCategory.IsSuccessStatusCode)
            {
                var jsonString = responseLocalizedCategory.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var localizedCategory = JsonConvert.DeserializeObject<Category>(jsonString);

                // Open prompt loop
                do
                {
                    WriteLine(localizedCategory.ToString());

                    // Prompt
                    SetCursorPosition(2, 5);
                    Write("Category Name: ");

                    SetCursorPosition(2, 6);
                    Write("Image Url: ");


                    // Set cursor to visible
                    CursorVisible = true;

                    // Set cursor position to each prompt subsequently & retrieve and validate respecitve information
                    int unifiedIdentation = "Category Name: ".Length + 3;

                    SetCursorPosition(unifiedIdentation, 5);
                    string name = ReadLine();

                    SetCursorPosition(unifiedIdentation, 6);
                    string imageUrlString = ReadLine();
                    bool imageUrlOk = Uri.TryCreate(imageUrlString, UriKind.Absolute, out Uri imageUrlVerified)
                                        && (imageUrlVerified.Scheme == Uri.UriSchemeHttp || imageUrlVerified.Scheme == Uri.UriSchemeHttps);

                    Uri imageUrl = imageUrlOk ? imageUrlVerified : new Uri("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");



                    // Further confirmation request
                    ConsoleKeyInfo confirmation = RequestConfirmation();

                    // Respond to confirmation choice: "Yes"
                    if (confirmation.Key == ConsoleKey.Y)
                    {
                        // Instantiate new Category object based on retrieved values
                        var category = new Category(id, name, imageUrl);

                        // Set TypeNameHandling to auto
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.TypeNameHandling = TypeNameHandling.Auto;

                        // Serialize to JSON
                        var httpContent = JsonConvert.SerializeObject(category, settings);

                        // Construct a content object to send the data
                        var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
                        var byteContent = new ByteArrayContent(buffer);

                        // Set the content type to JSON 
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        // Send put request
                        var response = httpClient.PutAsync($"categories/{id.ToString()}?api-version=1", byteContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("\n  Category Edited");
                        }
                        else
                        {
                            WriteLine("\n  Something went wrong while updating the category...");
                        }
                        ,
                        Thread.Sleep(2000);
                        Clear();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        Clear();

                        // Go back to prompt menu
                    }

                } while (true);
            }
            else
            {
                WriteLine("\n  Category not found...");
            }

            Thread.Sleep(2000);
            Clear();
        }
        private static void DeleteCakeCategory()
        {
            ListCakeCategories();

            Write("\n\tID: ");
            bool idValid = Int32.TryParse(ReadLine(), out int idParsed);
            int id = idValid ? idParsed : 0;

            var response = httpClient.DeleteAsync($"categories/{id.ToString()}?api-version=1")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                WriteLine("\n\tCategory Deleted");
            }
            else
            {
                WriteLine("\n\tSomething went wrong...");
            }

            Thread.Sleep(2000);

            Clear();
        }


        private static void ShowCakesMenu(bool isAdministrator)
        {
            bool applicationRunning = true;

            do
            {
                if (isAdministrator)
                {
                    WriteLine(
                              @"" +
                              "\n\t1. Register New Cake" +
                              "\n\t2. List Cakes" +
                              "\n\t3. Edit Cake" +
                              "\n\t4. Delete Cake" +
                              "\n\t5. Back");

                    ConsoleKeyInfo input = ReadKey(true);
                    Clear();

                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            RegisterNewCakeView();
                            break;

                        case ConsoleKey.D2:
                            ListCakes();
                            EscapeToPreviousMenu();
                            break;

                        case ConsoleKey.D3:
                            EditCake();
                            break;

                        case ConsoleKey.D4:
                            DeleteCake();
                            break;

                        case ConsoleKey.D5:
                            applicationRunning = false;
                            break;
                    }
                }

                else
                {
                    WriteLine(
                              @"" +
                              "\n\t1. List Cakes" +
                              "\n\t2. Back");

                    ConsoleKeyInfo input = ReadKey(true);
                    Clear();

                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            ListCakes();
                            EscapeToPreviousMenu();
                            break;

                        case ConsoleKey.D2:
                            applicationRunning = false;
                            break;
                    }
                }

            } while (applicationRunning);
        }
        private static void RegisterNewCakeView()
        {
            // Open prompt loop
            do
            {
                // Prompt 
                SetCursorPosition(2, 2);
                Write("Name: ");

                SetCursorPosition(2, 3);
                Write("Description: ");

                SetCursorPosition(2, 4);
                Write("Image Url: ");

                SetCursorPosition(2, 5);
                Write("Price: ");

                SetCursorPosition(2, 7);
                Write("Cake Category: ");


                // Set cursor to visible
                CursorVisible = true;

                // Set cursor position to each prompt subsequently & retrieve respecitve information
                int unifiedIdentation = "Description: ".Length + 3;

                SetCursorPosition(unifiedIdentation, 2);
                string name = ReadLine();

                SetCursorPosition(unifiedIdentation, 3);
                string description = ReadLine();

                SetCursorPosition(unifiedIdentation, 4);
                string imageUrlString = ReadLine();
                bool imageUrlOk = Uri.TryCreate(imageUrlString, UriKind.Absolute, out Uri imageUrlVerified)
                                    && (imageUrlVerified.Scheme == Uri.UriSchemeHttp || imageUrlVerified.Scheme == Uri.UriSchemeHttps);
                Uri imageUrl = imageUrlOk ? imageUrlVerified : new Uri("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");

                SetCursorPosition(unifiedIdentation, 5);
                bool priceOk = Int32.TryParse(ReadLine(), out int priceParsed);
                int price = priceOk ? priceParsed : 0;

                SetCursorPosition(unifiedIdentation, 7);
                string categoryName = ReadLine();


                // Further confirmation request
                ConsoleKeyInfo confirmation = RequestConfirmation();

                // Respond to confirmation choice: "Yes"
                if (confirmation.Key == ConsoleKey.Y)
                {
                    var categoryId = FetchCategoryIdByCategoryName(categoryName);

                    if (categoryId < 0)
                    {
                        WriteLine("\n  Indicated Category was not found...");
                        Thread.Sleep(2000);
                        Clear();
                        break;
                    }

                    // Instantiate new Cake object based on retrieved values
                    var cake = new Cake(name, description, imageUrl, price, categoryId);

                    // Set TypeNameHandling to auto
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.TypeNameHandling = TypeNameHandling.Auto;

                    // Serialize to JSON
                    var httpContent = JsonConvert.SerializeObject(cake, settings);

                    // Construct a content object to send the data
                    var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
                    var byteContent = new ByteArrayContent(buffer);

                    // Set the content type to JSON 
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    // Send post request
                    var response = httpClient.PostAsync("cakes?api-version=1", byteContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        WriteLine("\n  Cake Added");
                    }
                    else
                    {
                        WriteLine("\n  Something went wrong...");
                    }

                    Thread.Sleep(2000);
                    Clear();
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                    Clear();

                    // Go back to prompt menu
                }

            } while (true);
        }
        private static int FetchCategoryIdByCategoryName(string categoryName)
        {
            // HTTP GET https://localhost:44353/api/games
            var response = httpClient.GetAsync("categories?api-version=1")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);

                foreach (var category in categories)
                {
                    if (category.Name == categoryName)
                    {
                        return category.Id;
                    }
                }
            }
            return -1;
        }
        private static void ListCakes()
        {
            // HTTP GET https://localhost:44378/api/cakes
            var response = httpClient.GetAsync("cakes?api-version=1")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var cakes = JsonConvert.DeserializeObject<IEnumerable<Cake>>(jsonString);

                WriteLine("\n\tId\t\tName\t\tPrice");
                WriteLine("-------------------------------------------------------------");

                foreach (var cake in cakes)
                {
                    WriteLine($"\t{cake.Id}\t\t{cake.Name}\t\t{cake.Price}");
                }
            }
            else
            {
                WriteLine("\n\tSomething went wrong...");
            }
        }
        private static void EditCake()
        {
            ListCakes();

            Write("\n\tID: ");
            bool idValid = Int32.TryParse(ReadLine(), out int idParsed);
            int id = idValid ? idParsed : 0;

            var responseLocalizedCake = httpClient.GetAsync($"cakes/{id.ToString()}?api-version=1")
                .GetAwaiter()
                .GetResult();

            Clear();

            if (responseLocalizedCake.IsSuccessStatusCode)
            {
                var jsonString = responseLocalizedCake.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var localizedCake = JsonConvert.DeserializeObject<Cake>(jsonString);

                // Open prompt loop
                do
                {
                    WriteLine(localizedCake.ToString());

                    // Prompt
                    SetCursorPosition(2, 10);
                    Write("Name: ");

                    SetCursorPosition(2, 11);
                    Write("Description: ");

                    SetCursorPosition(2, 12);
                    Write("Image Url: ");

                    SetCursorPosition(2, 13);
                    Write("Price: ");


                    // Set cursor to visible
                    CursorVisible = true;

                    // Set cursor position to each prompt subsequently & retrieve respecitve information
                    int unifiedIdentation = "Description: ".Length + 3;

                    SetCursorPosition(unifiedIdentation, 10);
                    string name = ReadLine();

                    SetCursorPosition(unifiedIdentation, 11);
                    string description = ReadLine();

                    SetCursorPosition(unifiedIdentation, 12);
                    string imageUrlString = ReadLine();
                    bool imageUrlOk = Uri.TryCreate(imageUrlString, UriKind.Absolute, out Uri imageUrlVerified)
                                        && (imageUrlVerified.Scheme == Uri.UriSchemeHttp || imageUrlVerified.Scheme == Uri.UriSchemeHttps);
                    Uri imageUrl = imageUrlOk ? imageUrlVerified : new Uri("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");

                    SetCursorPosition(unifiedIdentation, 13);
                    bool priceOk = Int32.TryParse(ReadLine(), out int priceParsed);
                    int price = priceOk ? priceParsed : 0;



                    // Further confirmation request
                    ConsoleKeyInfo confirmation = RequestConfirmation();

                    // Respond to confirmation choice: "Yes"
                    if (confirmation.Key == ConsoleKey.Y)
                    {
                        // Instantiate new Cake object based on retrieved values
                        var cake = new Cake(id, name, description, imageUrl, price);

                        // Set TypeNameHandling to auto
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.TypeNameHandling = TypeNameHandling.Auto;

                        // Serialize to JSON
                        var httpContent = JsonConvert.SerializeObject(cake, settings);

                        // Construct a content object to send the data
                        var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
                        var byteContent = new ByteArrayContent(buffer);

                        // Set the content type to JSON 
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        // Send put request
                        var response = httpClient.PutAsync($"cakes/{id.ToString()}?api-version=1", byteContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("\n  Cake Edited");
                        }
                        else
                        {
                            WriteLine("\n  Something went wrong while updating the cake...");
                        }

                        Thread.Sleep(2000);
                        Clear();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        Clear();

                        // Go back to prompt menu
                    }

                } while (true);
            }
            else
            {
                WriteLine("\n  Cake not found...");
            }

            Thread.Sleep(2000);
            Clear();
        }
        private static void DeleteCake()
        {
            ListCakes();

            Write("\n\tID: ");
            bool idValid = Int32.TryParse(ReadLine(), out int idParsed);
            int id = idValid ? idParsed : 0;

            var response = httpClient.DeleteAsync($"cakes/{id.ToString()}?api-version=1")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                WriteLine("\n\tCake Deleted");
            }
            else
            {
                WriteLine("\n\tSomething went wrong...");
            }

            Thread.Sleep(2000);

            Clear();
        }


        private static void EscapeToPreviousMenu()
        {
            WriteLine("\n\tPress Esc to come back to previous menu");

            ConsoleKeyInfo input = ReadKey(true);

            while (input.Key != ConsoleKey.Escape)
            {
                input = ReadKey(true);
            }

            Clear();
        }
        private static ConsoleKeyInfo RequestConfirmation()
        {
            WriteLine("\n  Is this correct? (Y)es, (N)o");
            CursorVisible = false;

            ConsoleKeyInfo confirmation;
            bool confirmationOk;
            do
            {
                confirmation = ReadKey(true);
                confirmationOk = confirmation.Key == ConsoleKey.Y || confirmation.Key == ConsoleKey.N;

            } while (!confirmationOk);

            return confirmation;

        }

        //************************************************************************************//
        private static void ShowCorporatesMenu(bool isAdministrator)
        {
            bool applicationRunning = true;

            do
            {
                if (isAdministrator)
                {
                    WriteLine("1. Add new gift box");
                    WriteLine("2. List gift boxes");
                    WriteLine("3. Edit gift box");
                    WriteLine("4. Delete gift box");
                    WriteLine("5. Back");

                    ConsoleKeyInfo input = ReadKey(true);

                    Clear();

                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            AddNewGiftBox();
                            break;

                        case ConsoleKey.D2:
                            ListGiftBoxes();
                            break;


                        case ConsoleKey.D3:
                            EditGiftBox();
                            break;

                        case ConsoleKey.D4:
                            DeleteGiftBox();
                            break;

                        case ConsoleKey.D5:

                            applicationRunning = false;

                            break;
                    }
                }
                else
                {
                    WriteLine("1. List Corporate");
                    WriteLine("2. Back");

                    ConsoleKeyInfo input = ReadKey(true);
                    Clear();

                    switch (input.Key)
                    {
                        case ConsoleKey.D1:
                            ListCakeCategories();
                            EscapeToPreviousMenu();
                            break;

                        case ConsoleKey.D2:
                            applicationRunning = false;
                            break;
                    }
                }
            }

            while (applicationRunning);

        }
        private static int FetchCorporateIdByCorporateName(string corporateName)
        {
            // HTTP GET https://localhost:44353/api/games
            var response = httpClient.GetAsync("corporates?api-version=1")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var corporates = JsonConvert.DeserializeObject<IEnumerable<Corporate>>(jsonString);

                foreach (var corporate in corporates)
                {
                    if (corporate.Name == corporateName)
                    {
                        return corporate.Id;
                    }
                }
            }
            return -1;
        }
        private static void DeleteGiftBox()
        {
            ListGiftBoxes();

            Write("ID: ");
            bool idValid = Int32.TryParse(ReadLine(), out int idParsed);
            int id = idValid ? idParsed : 0;
            
            var response = httpClient.DeleteAsync($"corporate/{id.ToString()}?api-version=1")
                .GetAwaiter()
                .GetResult();

            Clear();
            if (response.IsSuccessStatusCode)
            {
                WriteLine("Corporate Deleted");
            }
            else
            {
                
                WriteLine("Something went wrong...");
            }

            Thread.Sleep(2000);

            Clear();
        }
        private static void EditGiftBox()
        {
            ListGiftBoxes();

            Write("ID: ");
            bool idValid = Int32.TryParse(ReadLine(), out int idParsed);
            int id = idValid ? idParsed : 0;

            var responseLocalizedCorporate = httpClient.GetAsync($"corporates/{id.ToString()}?api-version=1")
                .GetAwaiter()
                .GetResult();

            Clear();

            if (responseLocalizedCorporate.IsSuccessStatusCode)
            {
                var jsonString = responseLocalizedCorporate.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var localizedCorporate = JsonConvert.DeserializeObject<Corporate>(jsonString);

                // Open prompt loop
                do
                {
                    WriteLine(localizedCorporate.ToString());

                    // Prompt
                    SetCursorPosition(2, 5);
                    Write("Corporate Name: ");

                    SetCursorPosition(2, 6);
                    Write("Image Url: ");


                    // Set cursor to visible
                    CursorVisible = true;

                    // Set cursor position to each prompt subsequently & retrieve and validate respecitve information
                    int unifiedIdentation = "corporate Name: ".Length + 3;

                    SetCursorPosition(unifiedIdentation, 5);
                    string name = ReadLine();

                    SetCursorPosition(unifiedIdentation, 6);
                    string imageUrlString = ReadLine();
                    bool imageUrlOk = Uri.TryCreate(imageUrlString, UriKind.Absolute, out Uri imageUrlVerified)
                                        && (imageUrlVerified.Scheme == Uri.UriSchemeHttp || imageUrlVerified.Scheme == Uri.UriSchemeHttps);

                    Uri imageUrl = imageUrlOk ? imageUrlVerified : new Uri("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");



                    // Further confirmation request
                    ConsoleKeyInfo confirmation = RequestConfirmation();

                    // Respond to confirmation choice: "Yes"
                    if (confirmation.Key == ConsoleKey.Y)
                    {
                        // Instantiate new Category object based on retrieved values
                        var corporate = new Corporate(id, name, imageUrl);

                        // Set TypeNameHandling to auto
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.TypeNameHandling = TypeNameHandling.Auto;

                        // Serialize to JSON
                        var httpContent = JsonConvert.SerializeObject(corporate, settings);

                        // Construct a content object to send the data
                        var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
                        var byteContent = new ByteArrayContent(buffer);

                        // Set the content type to JSON 
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        // Send put request
                        var response = httpClient.PutAsync($"corporates/{id.ToString()}?api-version=1", byteContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("\n  Corporate Edited");
                        }
                        else
                        {
                            WriteLine("\n  Something went wrong ...");
                        }

                        Thread.Sleep(2000);
                        Clear();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        Clear();

                        // Go back to prompt menu
                    }

                } while (true);
            }
            else
            {
                WriteLine("\n  Corporate not found...");
            }

            Thread.Sleep(2000);
            Clear();
        }
        private static void ListGiftBoxes()
        {
            var response = httpClient.GetAsync("corporates?api-version=1")
               .GetAwaiter()
               .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                var corporates = JsonConvert.DeserializeObject<IEnumerable<Corporate>>(jsonString);

                WriteLine("Id \t\t\t Name");
                WriteLine("-------------------------------------------------------------");

                foreach (var corporate in corporates)
                {
                    WriteLine($"{corporate.Id} \t\t\t {corporate.Name}");
                }
                ReadKey(true);
            }
            else
            {
                WriteLine("\n\tSomething went wrong...");
                Thread.Sleep(2000);
                Clear();
            }
        }

        private static void AddNewGiftBox()
        {
            do
            {
                
                WriteLine("Corporate Name: ");

                
                WriteLine("Image Url: ");

              

                CursorVisible = true;

                SetCursorPosition(16, 0);
                string name = ReadLine();

                SetCursorPosition(11, 1);
                string imageUrlString = ReadLine();

                bool imageUrlOk = Uri.TryCreate(imageUrlString, UriKind.Absolute, out Uri imageUrlVerified)
                                    && (imageUrlVerified.Scheme == Uri.UriSchemeHttp || imageUrlVerified.Scheme == Uri.UriSchemeHttps);

                Uri imageUrl = imageUrlOk ? imageUrlVerified : new Uri("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");


                // Further confirmation request
                ConsoleKeyInfo confirmation = RequestConfirmation();

                if (confirmation.Key == ConsoleKey.Y)
                {
                    // Instantiate new Corporate object based on retrieved values
                    var corporate = new Corporate(name, imageUrl);

                    // Set TypeNameHandling to auto
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.TypeNameHandling = TypeNameHandling.Auto;

                    // Serialize to JSON
                    var httpContent = JsonConvert.SerializeObject(corporate, settings);

                    // Construct a content object to send the data
                    var buffer = System.Text.Encoding.UTF8.GetBytes(httpContent);
                    var byteContent = new ByteArrayContent(buffer);

                    // Set the content type to JSON 
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    // Send post request
                    var response = httpClient.PostAsync("corporates?api-version=1", byteContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        WriteLine("\n  Corporate Added");
                    }
                    else
                    {
                        WriteLine("\n  Something went wrong...");
                    }

                    Thread.Sleep(2000);
                    Clear();
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                    Clear();

                    // Go back to prompt menu
                }

            } while (true);
        }
    }
}