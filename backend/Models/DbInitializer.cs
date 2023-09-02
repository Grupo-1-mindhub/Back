using backend.Enumerates;

namespace backend.Models
{
    public class DbInitializer
    {
        public static void Initialize(MyContext context)
        {
            if (!context.PaymentMethods.Any())
            {
                var paymentMethods = new PaymentMethod[]
                {
                    new PaymentMethod {Description = PaymentMethodType.CASH.ToString() },
                    new PaymentMethod {Description = PaymentMethodType.DEBIT.ToString() },
                    new PaymentMethod {Description = PaymentMethodType.CREDIT.ToString() }
                };

                foreach (var paymentMethod in paymentMethods)
                {
                    context.PaymentMethods.Add(paymentMethod);
                }

                context.SaveChanges();

            }

            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category { Description = CategoryType.GENERAL.ToString() },
                    new Category { Description = CategoryType.FOOD.ToString() },
                    new Category { Description = CategoryType.ENTERTAINMENT.ToString() },
                    new Category { Description = CategoryType.SERVICES.ToString() },
                    new Category { Description = CategoryType.MARKET.ToString() },
                    new Category { Description = CategoryType.TRANSPORT.ToString() },
                };

                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }

                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client
                    {
                        FirstName = "Juan",
                        LastName = "Perez",
                        Password = "test",
                        Email = "juan@gmail.com"
                    },
                    new Client
                    {
                        FirstName = "Pedro",
                        LastName = "Sanchez",
                        Password = "test",
                        Email = "test@test.com"
                    }
                };
                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var clientJuan = context.Clients.FirstOrDefault(c => c.Email == "juan@gmail.com");
                if (clientJuan != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = "001", Balance = 10000, Description = "UALA"},
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = "002", Balance = 10000, Description = "ICBC"},
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = "003", Balance = 10000, Description = "BBVA"}
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();
            }

           

            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "001");
                var account2 = context.Accounts.FirstOrDefault(c => c.Number == "002");
                var account3 = context.Accounts.FirstOrDefault(c => c.Number == "003");
                var paymentMethod = context.PaymentMethods.FirstOrDefault(c => c.Id == 1);
                var category1 = context.Categories.FirstOrDefault(c => c.Id == 1);
                var category2 = context.Categories.FirstOrDefault(c => c.Id == 2);
                var category3 = context.Categories.FirstOrDefault(c => c.Id == 3);
                var category4 = context.Categories.FirstOrDefault(c => c.Id == 4);
                var category5 = context.Categories.FirstOrDefault(c => c.Id == 5);
                var category6 = context.Categories.FirstOrDefault(c => c.Id == 6);

                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        //Gastos UALA
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -2000, CreationDate = new DateTime(2023, 1, 15), Description = "Nafta" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -450, CreationDate = new DateTime(2023, 1, 17), Description = "Salida" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -1500, CreationDate = new DateTime(2023, 2, 10), Description = "Gasoil" },
                        new Transaction { CategoryId = category4.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -5000, CreationDate = new DateTime(2023, 3, 20), Description = "Aceite" },
                        new Transaction { CategoryId = category3.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -800, CreationDate = new DateTime(2023, 4, 5), Description = "Comida" },
                        new Transaction { CategoryId = category5.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -350, CreationDate = new DateTime(2023, 5, 12), Description = "Entretenimiento" },
                        new Transaction { CategoryId = category6.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -900, CreationDate = new DateTime(2023, 5, 22), Description = "Electrónicos" },
                        new Transaction { CategoryId = category4.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -1200, CreationDate = new DateTime(2023, 6, 8), Description = "Ropa" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -450, CreationDate = new DateTime(2023, 7, 17), Description = "Salida" },
                        new Transaction { CategoryId = category3.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -900, CreationDate = new DateTime(2023, 8, 22), Description = "Electrónicos" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -700, CreationDate = new DateTime(2023, 9, 6), Description = "Regalos" },
                        new Transaction { CategoryId = category4.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -300, CreationDate = new DateTime(2023, 10, 14), Description = "Libros" },
                        new Transaction { CategoryId = category6.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -550, CreationDate = new DateTime(2023, 11, 30), Description = "Café" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = -1000, CreationDate = new DateTime(2023, 12, 5), Description = "Vacaciones" },
                        //Ingresos UALA
                        
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = 15000, CreationDate =  new DateTime(2023, 1, 15), Description = "Deposito" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = 20000, CreationDate =  new DateTime(2023, 6, 8), Description = "Deposito" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account1.Id, Amount = 5000, CreationDate =  new DateTime(2023, 10, 14), Description = "Deposito" },
                        
                        //Gastos ICBC
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -2000, CreationDate = new DateTime(2023, 7, 15), Description = "Nafta" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -450, CreationDate = new DateTime(2023, 3, 17), Description = "Salida" },
                        new Transaction { CategoryId = category6.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -900, CreationDate = new DateTime(2023, 5, 22), Description = "Electrónicos" },
                        new Transaction { CategoryId = category4.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -1200, CreationDate = new DateTime(2023, 6, 8), Description = "Ropa" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -450, CreationDate = new DateTime(2023, 8, 17), Description = "Salida" },
                        new Transaction { CategoryId = category3.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -900, CreationDate = new DateTime(2023, 8, 22), Description = "Electrónicos" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -700, CreationDate = new DateTime(2023, 1, 6), Description = "Regalos" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = -1000, CreationDate = new DateTime(2023, 10, 5), Description = "Vacaciones" },
                        //Ingresos ICBC
                        
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = 7500, CreationDate =  new DateTime(2023, 7, 15), Description = "Deposito" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = 2000, CreationDate =  new DateTime(2023, 1, 8), Description = "Deposito" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account2.Id, Amount = 5000, CreationDate =  new DateTime(2023, 12, 14), Description = "Deposito" },


                         //Gastos BBVA
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -2000, CreationDate = new DateTime(2023, 6, 15), Description = "Nafta" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -450, CreationDate = new DateTime(2023, 3, 17), Description = "Salida" },
                        new Transaction { CategoryId = category6.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -900, CreationDate = new DateTime(2023, 5, 22), Description = "Electrónicos" },
                        new Transaction { CategoryId = category4.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -1200, CreationDate = new DateTime(2023, 6, 8), Description = "Ropa" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -450, CreationDate = new DateTime(2023, 3, 17), Description = "Salida" },
                        new Transaction { CategoryId = category3.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -900, CreationDate = new DateTime(2023, 7, 22), Description = "Electrónicos" },
                        new Transaction { CategoryId = category2.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -700, CreationDate = new DateTime(2023, 1, 6), Description = "Regalos" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = -1000, CreationDate = new DateTime(2023, 4, 5), Description = "Vacaciones" },
                        //Ingresos BBVA
                        
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = 7500, CreationDate =  new DateTime(2023, 2, 15), Description = "Deposito" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = 2000, CreationDate =  new DateTime(2023, 7, 8), Description = "Deposito" },
                        new Transaction { CategoryId = category1.Id, PaymentMethodId = paymentMethod.Id, AccountId = account3.Id, Amount = 5000, CreationDate =  new DateTime(2023, 5, 14), Description = "Deposito" },



                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }

            }
            
            if (!context.Cards.Any())
            {
                //buscamos al cliente 
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "test");
                if (client1 != null)
                {
                    //le agregamos 1 tarjeta de DEBITO
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client1.Id,
                            Type = CardType.DEBIT.ToString(),
                            Number = "3325-6745-7876-4445",
                            Deadline= DateTime.Now.AddYears(4),
                        },
                    };

                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
