namespace backend.Models
{
    public class DbInitializer
    {
        public static void Initialize(MyContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client
                    {
                        FirstName = "Juan",
                        LastName = "Perez",
                        Password = "Juan123",
                        Email = "juanp@hotmail.com"
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
                var clientJuan = context.Clients.FirstOrDefault(c => c.Email == "juanp@hotmail.com");
                if (clientJuan != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = "001", Balance = 10000, Description = "test"}
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();
            }

            if (!context.Budgets.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "001");
                if (account1 != null)
                {
                    var budgets = new Budget[]
                    {
                        new Budget {AccountId= account1.Id, Amount = 5000},
                        new Budget {AccountId= account1.Id, Amount = 1500}
                    };
                    foreach (Budget budget in budgets)
                    {
                        context.Budgets.Add(budget);
                    }
                    context.SaveChanges();
                }
            }

            if (context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { BudgetId = account1.Id, Amount = 2000, CreationDate = DateTime.Now, Description = "Viaje" }
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }

            }
        }
    }
}
