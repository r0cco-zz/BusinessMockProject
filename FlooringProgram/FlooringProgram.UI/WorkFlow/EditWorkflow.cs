using System;
using FlooringProgram.BLL;
using FlooringProgram.Models;

namespace FlooringProgram.UI.WorkFlow
{
    public class EditWorkflow
    {
        OrderOperations Ops { get; set; }
        public EditWorkflow(OrderOperations orderOperations)
        {
            Ops = orderOperations;
        }

        public void Execute()
        {
            string orderDate = GetOrderDateFromUser();
           
            if (orderDate != "X")
            {
                int orderNumber = GetOrderNumberFromUser();

                Response response = Ops.EditOrder(orderDate, orderNumber);
                if (response.Success)
                {
                    Response ultimateEdit = DisplayOrder(response);
                    if (ultimateEdit != null)
                    { FinalDisplay(ultimateEdit);}
                }
                else
                {
                    Console.WriteLine("Sorry, there was no order found matching that data...");
                    Console.WriteLine("Press enter to return to the main menu...");
                    Console.ReadLine();
                }
            }
        }

        public string GetOrderDateFromUser()
        {
            string input;
            do
            {
                Console.Clear();
                DateTime orderDate;
                Console.Write("Enter an order date: ");
                var orderDateString = Console.ReadLine();
                bool validDate = DateTime.TryParse(orderDateString, out orderDate);


                bool doesExist = Ops.DoesDateExistBLL(orderDate.ToString("MMddyyyy")); //here
                if (doesExist && validDate)
                {

                    DisplayAllOrdersFromDate(orderDate.ToString("MMddyyyy"));
                    return orderDate.ToString("MMddyyyy");

                }
                if (!validDate)
                {
                    Console.WriteLine("That does not look like a valid date...");
                    var log = new ErrorLogger()
                    {
                        TimeOfError = DateTime.Now,
                        Message = String.Format("EditOrder : invalid date entered : {0}", orderDateString)
                    };
                    Ops.ErrorPassdown(log);
                }

                if (validDate)
                    Console.WriteLine("There are no matching orders...");
                Console.WriteLine("Press enter to continue, or (M)ain Menu...");

                var log2 = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = $"EditOrder : no orders on date entered : {orderDateString}"
                };
                Ops.ErrorPassdown(log2);

                input = Console.ReadLine();
                input = input?.ToUpper();
                if (input != null && input.ToUpper() == "M")
                {
                    return "X";
                }
            } while (true);

        }

        public int GetOrderNumberFromUser()
        {
            do
            {
                //Console.Clear();
                int orderNumber;

                Console.Write("Enter an order number to edit : ");
                var orderNumberString = Console.ReadLine();
                //bool doesExist = File.Exists(String.Format(@"DataFiles\Orders_{0}.txt", orderDateString));
                if (int.TryParse(orderNumberString, out orderNumber))
                {
                    return orderNumber;
                }

                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = String.Format("EditOrder : invalid order number entered : {0}", orderNumber)
                };
                Ops.ErrorPassdown(log);

                Console.WriteLine("Please enter a number to check for an order.");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (true);
        }

        public Response DisplayOrder(Response response)
        {
            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine("Order date : {0}/{1}/{2}", response.Order.OrderDate.Substring(0, 2),
                    response.Order.OrderDate.Substring(2, 2),
                    response.Order.OrderDate.Substring(4));
                Console.WriteLine("Order number {0:0000}", response.Order.OrderNumber);
                Console.WriteLine("\nCustomer name : {0}", response.Order.CustomerName);
                Console.WriteLine("Area : {0} sq ft", response.Order.Area);
                Console.WriteLine("Product type : {0}", response.Order.ProductType.ProductType);
                Console.WriteLine("Material cost per sq ft : {0:c}", response.Order.ProductType.MaterialCost);
                Console.WriteLine("Labor cost per sq ft : {0:c}", response.Order.ProductType.LaborCost);
                Console.WriteLine("Total material cost : {0:c}", response.Order.MaterialCost);
                Console.WriteLine("Total labor cost : {0:c}", response.Order.LaborCost);
                Console.WriteLine("{0} state tax ({1:p}) : {2:c}", response.Order.State, response.Order.TaxRate/100,
                    response.Order.Tax);
                Console.WriteLine("\nOrder total : {0:c}", response.Order.Total);
                Console.WriteLine();
                Console.WriteLine();

                Response orderToEdit = ConfirmUserEdit(response);
                return orderToEdit;

                //method to take this input and use it to either write on the data or not
            }
            if (!response.Success)
            {
                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = $"EditOrder : error displaying order data from selected date : {response.Message}"
                };
                Ops.ErrorPassdown(log);

                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();

            }
            return null;
        }

        public Response ConfirmUserEdit(Response orderInfo)
        {
            Console.Write("Is this the order you wish to edit (y/n) ? : ");
            string input = Console.ReadLine();

            if (input != null && (input.ToUpper() == "Y" || input.ToUpper() == "YES"))
            {
                GetNewUserName(orderInfo);
                GetNewUserState(orderInfo);
                GetNewUserProductType(orderInfo);
                GetNewUserArea(orderInfo);
                Response response = Ops.EditedOrder(orderInfo);
                return response;
            }
            else
            {
                Console.WriteLine("No changes will be made to this order.");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
                return null;
            }
        }

        public Response GetNewUserName(Response orderInfo)
        {
            Console.WriteLine("Press enter if no change...");
            Console.Write("Enter new customer name ({0}) : ", orderInfo.Order.CustomerName);
            var newName = Console.ReadLine();
            if (newName != "")
            {
                orderInfo.Order.CustomerName = newName;
            }
            return orderInfo;
        }

        public Response GetNewUserState(Response orderInfo)
        {
            do
            {
                Console.WriteLine("Press enter if no change...");
                Console.Write("Enter new state (OH, PA, MI, or IN) as 2-letter abbreviation ({0}) : ", orderInfo.Order.State);
                var newState = Console.ReadLine();
                newState = newState?.ToUpper();
                if (newState == null) continue;
                switch (newState.ToUpper())
                {
                    case "":
                        return orderInfo;
                    case "OH":
                        orderInfo.Order.State = "OH";
                        orderInfo.Order.TaxRate = (decimal)6.25;
                        return orderInfo;
                    case "IN":
                        orderInfo.Order.State = "IN";
                        orderInfo.Order.TaxRate = (decimal) 6.00;
                        return orderInfo;
                    case "PA":
                        orderInfo.Order.State = "PA";
                        orderInfo.Order.TaxRate = (decimal) 6.75;
                        return orderInfo;
                    case "MI":
                        orderInfo.Order.State = "MI";
                        orderInfo.Order.TaxRate = (decimal) 5.75;
                        return orderInfo;
                    default:
                        var log = new ErrorLogger()
                        {
                            TimeOfError = DateTime.Now,
                            Message = String.Format("EditOrder : invalid state entered : {0}", newState)
                        };
                        Ops.ErrorPassdown(log);
                        break;
                }
            } while (true);
        }

        public Response GetNewUserProductType(Response orderInfo)
        {
            do
            {
                Console.WriteLine("Press enter if no change...");
                Console.Write("Enter new product type (Choices : (C)arpet, (L)aminate, (T)ile, (W)ood) ({0}) : ", orderInfo.Order.ProductType.ProductType);
                var newProductType = Console.ReadLine();
                if (newProductType == null) continue;
                switch (newProductType.ToUpper())
                {
                    case "":
                        return orderInfo;
                    case "C":
                    case "CARPET":
                        orderInfo.Order.ProductType = new ProductTypes()
                        {
                            ProductType = "Carpet",
                            MaterialCost = (decimal)2.25,
                            LaborCost = (decimal)2.10
                        };
                        return orderInfo;
                    case "L":
                    case "LAMINATE":
                        orderInfo.Order.ProductType = new ProductTypes()
                        {
                            ProductType = "Laminate",
                            MaterialCost = (decimal)1.75,
                            LaborCost = (decimal)2.10
                        };
                        return orderInfo;
                    case "T":
                    case "TILE":
                        orderInfo.Order.ProductType = new ProductTypes()
                        {
                            ProductType = "Tile",
                            MaterialCost = (decimal)3.50,
                            LaborCost = (decimal)4.15
                        };
                        return orderInfo;
                    case "W":
                    case "WOOD":
                        orderInfo.Order.ProductType = new ProductTypes()
                        {
                            ProductType = "Wood",
                            MaterialCost = (decimal) 5.15,
                            LaborCost = (decimal) 4.75
                        };
                        return orderInfo;
                    default:
                        var log = new ErrorLogger()
                        {
                            TimeOfError = DateTime.Now,
                            Message = String.Format("EditOrder : invalid product type entered : {0}", newProductType)
                        };
                        Ops.ErrorPassdown(log);
                        break;
                }
            } while (true);
        }

        public Response GetNewUserArea(Response orderInfo)
        {
            do
            {
                decimal newArea;
                Console.WriteLine("Press enter if no change...");
                Console.Write("Enter new area ({0}) : ", orderInfo.Order.Area);
                var newAreaString = Console.ReadLine();
                bool validArea = decimal.TryParse(newAreaString, out newArea);
                if (newAreaString != "" && validArea && newArea > 0)
                {
                    orderInfo.Order.Area = newArea;
                    return orderInfo;
                }
                else if (newAreaString == "")
                {
                    return orderInfo;
                }
                else if (!validArea)
                {
                    var log = new ErrorLogger()
                    {
                        TimeOfError = DateTime.Now,
                        Message = String.Format("EditOrder : invalid area value entered : {0}", newAreaString)
                    };
                    Ops.ErrorPassdown(log);
                }
                else if (newArea <= 0)
                {
                    var log = new ErrorLogger()
                    {
                        TimeOfError = DateTime.Now,
                        Message = String.Format("EditOrder : negative or zero area entered : {0}", newArea)
                    };
                    Ops.ErrorPassdown(log);
                }

            } while (true);
        }

        public void FinalDisplay(Response response)
        {
            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine("Order date : {0}/{1}/{2}", response.Order.OrderDate.Substring(0, 2),
                    response.Order.OrderDate.Substring(2, 2),
                    response.Order.OrderDate.Substring(4));
                Console.WriteLine("Order number {0:0000}", response.Order.OrderNumber);
                Console.WriteLine("\nCustomer name : {0}", response.Order.CustomerName);
                Console.WriteLine("Area : {0} sq ft", response.Order.Area);
                Console.WriteLine("Product type : {0}", response.Order.ProductType.ProductType);
                Console.WriteLine("Material cost per sq ft : {0:c}", response.Order.ProductType.MaterialCost);
                Console.WriteLine("Labor cost per sq ft : {0:c}", response.Order.ProductType.LaborCost);
                Console.WriteLine("Total material cost : {0:c}", response.Order.MaterialCost);
                Console.WriteLine("Total labor cost : {0:c}", response.Order.LaborCost);
                Console.WriteLine("{0} state tax ({1:p}) : {2:c}", response.Order.State, response.Order.TaxRate/100,
                    response.Order.Tax);
                Console.WriteLine("\nOrder total : {0:c}", response.Order.Total);
                Console.WriteLine();
                Console.WriteLine();
                ConfirmFinalLastEdit(response);

                //method to take this input and use it to either write on the data or not
            }
            if (!response.Success)
            {
                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = $"EditOrder : error displaying order info for final validation : {response.Message}"
                };
                Ops.ErrorPassdown(log);

                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }

        public void ConfirmFinalLastEdit(Response response)
        {
            Console.Write("Do you wish to save these changes to the order (y/n) ? : ");
            var input = Console.ReadLine();
            if (input != null && (input.ToUpper() == "Y" || input.ToUpper() == "YES"))
            {
                Ops.PassEditBll(response);
            }
            else
            {
                Console.WriteLine("No changes will be made to this order.");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }

        public void DisplayAllOrdersFromDate(string orderDate)
        {
            var response = Ops.GetAllOrdersFromDate(orderDate);

            if (response.Success)
            {
                foreach (var order in response.OrderList)
                {
                    Console.WriteLine("Order number {0}", order.OrderNumber);
                    Console.WriteLine("\t{0}, Total : {1:c}\n", order.CustomerName, order.Total);
                }


            }
            if (!response.Success)
            {
                var log = new ErrorLogger()
                {
                    TimeOfError = DateTime.Now,
                    Message = String.Format("EditOrder : error displaying all orders on selected date : {0}", response.Message)
                };
                Ops.ErrorPassdown(log);

                Console.Clear();
                Console.WriteLine("There was an error");
                Console.WriteLine("Press enter to return to main menu");
                Console.ReadLine();
            }
        }
    }
}
