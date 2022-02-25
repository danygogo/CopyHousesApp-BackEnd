using Microsoft.AspNetCore.Mvc;
using ApiForSale.Data;
using ApiForSale.Models;
using ApiForSale.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace ApiForSale.Controllers
{
    [Produces("application/json")]
    [Route("api/properties")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly Database context;

        public PropertiesController(Database context)
        {
            this.context = context;
        }


        //shows the entire table filtered by city
        [HttpGet]
        [Route("{city}")]
        public IActionResult GetByCity(string city)
        {

            List<Properties> propertiesList = new List<Properties>();

            propertiesList = context.Properties.Where(p => p.City.Equals(city)).ToList();

            if (propertiesList.Count == 0)
            {
                return NotFound();
            }
            return Ok(propertiesList);
        }




        //Shows a house's details, it has a inner join with the vendor's table to get the name, phone and mail
        [HttpGet]
        [Route("details/{id}")]
        public IActionResult GetDetails(int id)
        {

            string connString = context.Database.GetConnectionString().ToString();
            HouseDetails houseDetails = new HouseDetails();

            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //set stored procedure name
                    string spName = @"dbo.[getDetails]";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    //Set SqlParameters

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@IdRequired";
                    param1.SqlDbType = System.Data.SqlDbType.Int;
                    param1.Value = id;



                    //add the parameter to the SqlCommand object
                    cmd.Parameters.Add(param1);


                    //open connection
                    conn.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();


                    

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            houseDetails.Name = dr.GetString(0);
                            houseDetails.Mail = dr.GetString(1);
                            houseDetails.Phone = dr.GetString(2);
                            houseDetails.Id = dr.GetInt32(3);
                            houseDetails.Title = dr.GetString(4);
                            houseDetails.LandSize = dr.GetDouble(5);
                            houseDetails.ConstructionSize = dr.GetDouble(6); ;
                            houseDetails.lat = dr.GetDouble(7);
                            houseDetails.lng = dr.GetDouble(8);
                            houseDetails.KitchenAndLiving = dr.GetBoolean(9);
                            houseDetails.Price = dr.GetDouble(10);
                            houseDetails.Bedrooms = dr.GetInt32(11);
                            houseDetails.HasPool = dr.GetBoolean(12);
                            houseDetails.Details = dr.GetString(13);
                            houseDetails.City = dr.GetString(14);
                            houseDetails.Parkings = dr.GetInt32(15);
                            houseDetails.UserId = dr.GetInt32(16);
                            houseDetails.Sold = dr.GetBoolean(17);
                            houseDetails.Bathrooms = dr.GetInt32(26);

                            for (int i = 18; i <= 25; i++)
                            {
                                try
                                {
                                    byte[] data = (byte[])dr.GetValue(i);

                                    switch (i)
                                    {
                                        case 18:
                                            houseDetails.Photo1 = getImage(data);
                                            break;
                                        case 19:
                                            houseDetails.Photo2 = getImage(data);
                                            break;
                                        case 20:
                                            houseDetails.Photo3 = getImage(data);
                                            break;
                                        case 21:
                                            houseDetails.Photo4 = getImage(data);
                                            break;
                                        case 22:
                                            houseDetails.Photo5 = getImage(data);
                                            break;
                                        case 23:
                                            houseDetails.Photo6 = getImage(data);
                                            break;
                                        case 24:
                                            houseDetails.Photo7 = getImage(data);
                                            break;
                                        case 25:
                                            houseDetails.Photo8 = getImage(data);
                                            break;
                                    }


                                }
                                catch (Exception ex)
                                {
                                    switch (i)
                                    {
                                        case 18:
                                            houseDetails.Photo1 = null;
                                            break;
                                        case 19:
                                            houseDetails.Photo2 = null;
                                            break;
                                        case 20:
                                            houseDetails.Photo3 = null;
                                            break;
                                        case 21:
                                            houseDetails.Photo4 = null;
                                            break;
                                        case 22:
                                            houseDetails.Photo5 = null;
                                            break;
                                        case 23:
                                            houseDetails.Photo6 = null;
                                            break;
                                        case 24:
                                            houseDetails.Photo7 = null;
                                            break;
                                        case 25:
                                            houseDetails.Photo8 = null;
                                            break;
                                    }
                                }

                            }//end of for
                        }

                    }
                 


                    //close data reader
                    dr.Close();

                    //close connection
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                return Ok(houseDetails);//returns an empty object 
            }


            return Ok(houseDetails);
        }




         string getImage(byte[] data)
        {
            try
            {
                string base64String = Convert.ToBase64String(data, 0, data.Length);
                return "data:image/png;base64," + base64String;

            }
            catch (Exception ex)
            {
                return null;
            }
        }




        //Shows the houses by city and filtered using a stored procedure,
        //the idea is to get a smaller response to show in the houses menu
        [HttpGet]
        [Route("filter/{city=city}/{price?}")]
        public IActionResult GetFilteredByCity(string city, double price)
        {

            string connString = context.Database.GetConnectionString().ToString();
            List<spByCity> list = new List<spByCity>();

            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //set stored procedure name
                    string spName = @"dbo.[FilterByCityAndPrice]";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    //Set SqlParameters
                    List<SqlParameter> prm = new List<SqlParameter>();


                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@CityReceived";
                    param1.SqlDbType = System.Data.SqlDbType.NVarChar;
                    param1.Value = city;

                    
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@PriceReceived";
                    param2.SqlDbType = System.Data.SqlDbType.Float;
                    param2.Value = price;

                    //add the parameter to the SqlCommand object
                    prm.Add(param1);
                    prm.Add(param2);
                    Console.WriteLine(prm);
                    cmd.Parameters.AddRange(prm.ToArray());


                    //open connection
                    conn.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();



                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            spByCity houseFiltered = new spByCity();

                            try
                            {
                                byte[] data = (byte[])dr.GetValue(0);
                                string base64String = Convert.ToBase64String(data, 0, data.Length);
                                houseFiltered.Photo1 = "data:image/png;base64," + base64String;

                            }
                            catch (Exception ex)
                            {
                                houseFiltered.Photo1 = null;
                            }


                            houseFiltered.Title = dr.GetString(1);
                            houseFiltered.City = dr.GetString(2);
                            houseFiltered.Price = dr.GetDouble(3);
                            houseFiltered.Id = dr.GetInt32(4);
                            houseFiltered.Bedrooms = dr.GetInt32(5);
                            houseFiltered.Bathrooms = dr.GetInt32(6);
                            list.Add(houseFiltered);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }

                    //close data reader
                    dr.Close();

                    //close connection
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }


            return Ok(list);
        }





        //Shows the houses that a user is selling
        [HttpGet]
        [Route("filterByOwner/{id}")]
        public IActionResult dashboardList(int id)
        {

            string connString = context.Database.GetConnectionString().ToString();
            List<dbList> list = new List<dbList>();

            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //set stored procedure name
                    string spName = @"dbo.[vendorsProperties]";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    //Set SqlParameters
                    SqlParameter prm = new SqlParameter();


                    prm.ParameterName = "@IdUser";
                    prm.SqlDbType = System.Data.SqlDbType.Int;
                    prm.Value = id;


                    cmd.Parameters.Add(prm);


                    //open connection
                    conn.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();



                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            dbList houseFiltered = new dbList();

                            try
                            {
                                byte[] data = (byte[])dr.GetValue(0);
                                string base64String = Convert.ToBase64String(data, 0, data.Length);
                                houseFiltered.Photo1 = "data:image/png;base64," + base64String;

                            }
                            catch (Exception ex)
                            {
                                houseFiltered.Photo1 = null;
                            }


                            houseFiltered.Title = dr.GetString(1);

                            houseFiltered.Id = dr.GetInt32(2);
                            houseFiltered.Sold = dr.GetBoolean(3);
                            list.Add(houseFiltered);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }

                    //close data reader
                    dr.Close();

                    //close connection
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }


            return Ok(list);
        }









        //shows the entire table
        [HttpGet]
        [Route("showall")]
        public IEnumerable<Properties> Get()
        {
            List<Properties> propertiesList = new List<Properties>();
            Console.WriteLine(propertiesList);
            propertiesList = context.Properties.ToList();
            return propertiesList;
        }




        //this method allows to post the house
        [HttpPost]
        public IActionResult Add_Property([FromBody] PropertiesDataReceived propertyReceived)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Properties property = new Properties();

                    if (propertyReceived.Photo1 != null)
                    {
                        property.Photo1 = convertToArray(propertyReceived.Photo1);
                    }

                    if (propertyReceived.Photo2 != null)
                    {
                        property.Photo2 = convertToArray(propertyReceived.Photo2);
                    }

                    if (propertyReceived.Photo3 != null)
                    {
                        property.Photo3 = convertToArray(propertyReceived.Photo3);
                    }


                    if (propertyReceived.Photo4 != null)
                    {
                        property.Photo4 = convertToArray(propertyReceived.Photo4);
                    }

                    if (propertyReceived.Photo5 != null)
                    {
                        property.Photo5 = convertToArray(propertyReceived.Photo5);
                    }

                    if (propertyReceived.Photo6 != null)
                    {
                        property.Photo6 = convertToArray(propertyReceived.Photo6);
                    }

                    if (propertyReceived.Photo7 != null)
                    {
                        property.Photo7 = convertToArray(propertyReceived.Photo7);
                    }

                    if (propertyReceived.Photo8 != null)
                    {
                        property.Photo8 = convertToArray(propertyReceived.Photo8);
                    }

                    property.Title = propertyReceived.Title;
                    property.LandSize = propertyReceived.LandSize;
                    property.ConstructionSize = propertyReceived.ConstructionSize;
                    property.lat = propertyReceived.lat;
                    property.lng = propertyReceived.lng;
                    property.KitchenAndLiving = propertyReceived.KitchenAndLiving;
                    property.Price = propertyReceived.Price;
                    property.Bedrooms = propertyReceived.Bedrooms;
                    property.HasPool = propertyReceived.HasPool;
                    property.Details = propertyReceived.Details;
                    property.City = propertyReceived.City;
                    property.Parkings = propertyReceived.Parkings;
                    property.UserId = propertyReceived.UserId;
                    property.Sold = propertyReceived.Sold;
                    property.Bathrooms = propertyReceived.Bathrooms;


                    context.Properties.Add(property);
                    context.SaveChanges();


                    Seen seenBy = new Seen();
                    seenBy.SeenQuantity = 0;
                    seenBy.IdUser = property.UserId;
                    seenBy.IdProperty = property.Id;

                    context.Seen.Add(seenBy);
                    context.SaveChanges();



                    return Ok(true);
                }
                catch(Exception ex)
                {
                    return Ok(false);
                }
                
            }

            return Ok(false);
        }





        //Method to convert base64 to byte[]
        private byte[] convertToArray( String photoReceived )
        {
            try
            {
                byte[] newBytes = Convert.FromBase64String(photoReceived);
                return newBytes;

            }
            catch (Exception ex)
            {
                try
                {
                    photoReceived = photoReceived.Replace("=", "");
                    byte[] newBytes = Convert.FromBase64String(photoReceived);
                    return newBytes;

                }
                catch (Exception ex2)
                {
                    return null;
                }
            }
        }



        //this method allows to modify the house ad
        //TODO: modify it and check if the photo is working, this will be done for v2
        [HttpPut]
        public IActionResult Edit_Propery([FromBody] Properties property)
        {

            if (ModelState.IsValid)
            {
                context.Entry(property).State = EntityState.Modified;
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }







        //Shows the houses that a user is selling
        [HttpPut]
        [Route("sold")]
        public IActionResult updateAsSold([FromBody] int id)
        {

            string connString = context.Database.GetConnectionString().ToString();


            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //set stored procedure name
                    string spName = @"dbo.[setAsSold]";

                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, conn);

                    //Set SqlParameters
                    SqlParameter prm = new SqlParameter();


                    prm.ParameterName = "@IdProperty";
                    prm.SqlDbType = System.Data.SqlDbType.Int;
                    prm.Value = id;


                    cmd.Parameters.Add(prm);


                    //open connection
                    conn.Open();

                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(false);
            }

        }
    }

}