using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models.DAL
{
    public class DBInitialiser : DropCreateDatabaseAlways<LTCContext>
    {
        protected override void Seed(LTCContext context)
        {
            base.Seed(context);


            if (!context.Users.Any())
            {

                //###################  ROLES ##############################

                RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                //TOP TEIR ROLES
                if (!rm.RoleExists("Admin"))
                {
                    rm.Create(new IdentityRole("Admin"));
                }
                if (!rm.RoleExists("Author"))
                {
                    rm.Create(new IdentityRole("Author"));
                }
                if (!rm.RoleExists("Moderator"))
                {
                    rm.Create(new IdentityRole("Moderator"));
                }
                if (!rm.RoleExists("Editor"))
                {
                    rm.Create(new IdentityRole("Editor"));
                }

                //SUB TIER
                if (!rm.RoleExists("Reviewer"))
                {
                    rm.Create(new IdentityRole("Reviewer"));
                }

                if (!rm.RoleExists("Blogger"))
                {
                    rm.Create(new IdentityRole("Blogger"));
                }

                if (!rm.RoleExists("Reporter"))
                {
                    rm.Create(new IdentityRole("Reporter"));
                }
                
                //Basic user
                if (!rm.RoleExists("User"))
                {
                    rm.Create(new IdentityRole("User"));
                }

                //if user is suspended from logging into the platform
                if (!rm.RoleExists("Suspended"))
                {
                    rm.Create(new IdentityRole("Suspended"));
                }

                //if the user is blocked from making any new comments
                if (!rm.RoleExists("Silenced"))
                {
                    rm.Create(new IdentityRole("Silenced"));
                }
                

                context.SaveChanges();


                //###################  USERS ##############################
                UserManager<User> um = new UserManager<User>(new UserStore<User>(context));

                if (um.FindByName("admin@ltc.co.uk") == null)
                {
                    //badges
                    var adminBadge = new Badge
                    {
                        BadgeName = "Admin Badge",
                        Description = "This user is an epic Administrator",
                        ImageLocation = "/Content/Images/Badges/admin.png"
                    };
                    var authorBadge = new Badge
                    {
                        BadgeName = "Author Badge",
                        Description = "This user is a legendary Author",
                        ImageLocation = "/Content/Images/Badges/author.png"
                    };
                    var moderatorBadge = new Badge
                    {
                        BadgeName = "Moderator Badge",
                        Description = "This user is a badass Moderator",
                        ImageLocation = "/Content/Images/Badges/moderator.png"
                    };
                    var bloggerBadge = new Badge
                    {
                        BadgeName = "Blogger Badge",
                        Description = "This user is the speediest Blogger",
                        ImageLocation = "/Content/Images/Badges/blogger.png"
                    };
                    var reporterBadge = new Badge
                    {
                        BadgeName = "Reporter Badge",
                        Description = "This user is one smart Reporter",
                        ImageLocation = "/Content/Images/Badges/reporter.png"
                    };
                    var reviewerBadge = new Badge
                    {
                        BadgeName = "Reviewer Badge",
                        Description = "This user is a rocking reviewer",
                        ImageLocation = "/Content/Images/Badges/reviewer.png"
                    };

                    //week password validator
                    um.PasswordValidator = new PasswordValidator
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };

                    //ADMIN

                    var admin = new Admin
                    {
                        UserName = "admin@ltc.co.uk",
                        Email = "admin@ltc.co.uk",
                        FirstName = "Adam",
                        LastName = "Jenson",
                        StaffSince = DateTime.Now.AddYears(-3),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        UserImage = "/Content/Images/Users/Profiles/Demo/admin.jpg",
                        Badges = new List<Badge> { adminBadge, authorBadge, moderatorBadge },
                        Biography = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla nec ex mi. Aliquam quis sapien a massa cursus euismod. Maecenas malesuada mauris tincidunt, fermentum libero et, vehicula nunc. Suspendisse quis feugiat ante. Aliquam nunc urna, rutrum sed quam non, consectetur lobortis leo. Vestibulum pretium turpis et hendrerit lacinia. Donec tempus fermentum vulputate. Phasellus at consequat libero. Etiam tincidunt, libero eu sollicitudin volutpat, ex nulla cursus libero, in mattis leo nisi id nulla. Nam quis tincidunt purus. Ut fringilla velit risus, sed iaculis velit laoreet quis. Etiam hendrerit a dui quis convallis. Suspendisse potenti. Integer bibendum venenatis neque, at vulputate risus varius sit amet. Maecenas tempor sed tortor ac egestas. Vivamus et interdum tortor, ac accumsan tortor.</p>" +
                        "<p>Aenean dictum scelerisque efficitur. Vestibulum finibus sem vitae sapien tristique egestas. Vestibulum ac molestie nunc. Morbi eget sagittis lacus, eu eleifend ipsum. Vestibulum tincidunt lectus odio. Donec convallis interdum lorem, quis lacinia augue porta vitae. Proin eros quam, condimentum ac lacinia sit amet, interdum vehicula eros. Donec placerat est ac gravida interdum.</p>" +
                        "<p>Suspendisse potenti. Pellentesque quis mollis sapien. Nunc euismod enim nec tellus interdum, nec fringilla tortor ultrices. Nulla fringilla vitae ex ac congue. Aenean tincidunt lectus id augue auctor, a aliquam lectus pharetra. Aenean vitae arcu nibh. Mauris eget tortor a massa malesuada egestas. Pellentesque dapibus nisi at elit fringilla, ut venenatis orci pulvinar. Aenean suscipit imperdiet molestie. Nam metus lectus, hendrerit vitae facilisis vitae, aliquet ac magna. Praesent ac congue nisi. Curabitur maximus metus justo, sit amet tempus diam tristique in. Vestibulum ut justo facilisis, venenatis sapien vel, tincidunt ante.</p>" +
                        "<p>Maecenas in enim nulla. Nulla nulla magna, hendrerit vel facilisis nec, mattis nec quam. In quis massa auctor, iaculis risus congue, mattis justo. Etiam ultrices purus ut ultrices auctor. Etiam bibendum quis justo vel congue. Mauris nec semper justo. Vestibulum dapibus vehicula justo, eu porttitor massa efficitur sit amet. Donec finibus libero a justo vestibulum tristique. Nulla eget metus metus. Sed eget blandit justo. Fusce ac nibh at risus posuere pretium. Aliquam id eleifend tellus.</p>" +
                        "<p>Fusce tellus dolor, sagittis rutrum odio a, vehicula eleifend lorem. Maecenas lacinia interdum malesuada. Cras dapibus felis enim, et tristique sem mollis a. Donec suscipit, arcu a maximus feugiat, nulla ipsum iaculis nulla, vel feugiat urna massa eu quam. Duis dignissim condimentum ex, sit amet pretium metus maximus eget. Suspendisse metus nisi, eleifend at tempor nec, semper et ipsum. Proin vitae sapien nec quam varius congue vitae fermentum elit. Curabitur sed lobortis odio. Phasellus semper porta ligula eu molestie. Aenean fermentum nisl blandit, bibendum arcu et, gravida purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce sed blandit felis. Mauris non lectus efficitur, efficitur erat vitae, viverra ipsum.</p>"

                    };

                    um.Create(admin, "admin123");
                    um.AddToRole(admin.Id, "Admin");
                    um.AddToRole(admin.Id, "User");
                    um.AddToRole(admin.Id, "Blogger");

                    //MODS are 
                    var moderator = new Admin
                    {
                        UserName = "mod@ltc.co.uk",
                        Email = "mod@ltc.co.uk",
                        FirstName = "Charlie",
                        LastName = "Megasuarus",
                        StaffSince = DateTime.Now.AddYears(-1),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        UserImage = "/Content/Images/Users/Profiles/Demo/mod.jpg",
                        Badges = new List<Badge> {  moderatorBadge }
                    };

                    um.Create(moderator, "mod123");
                    um.AddToRole(moderator.Id, "User");
                    um.AddToRole(moderator.Id, "Moderator");

                    //AUTHORS!
                    var author1 = new Author
                    {
                        UserName = "author@ltc.co.uk",
                        Email = "author@ltc.co.uk",
                        FirstName = "Sandra",
                        LastName = "Bullock",
                        JoinDate = DateTime.Now.AddYears(-3),
                        StaffSince = DateTime.Now.AddYears(-3),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        UserImage = "/Content/Images/Users/Profiles/Demo/author.jpg",
                        Badges = new List<Badge> { authorBadge, reviewerBadge, bloggerBadge },
                        Biography = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla nec ex mi. Aliquam quis sapien a massa cursus euismod. Maecenas malesuada mauris tincidunt, fermentum libero et, vehicula nunc. Suspendisse quis feugiat ante. Aliquam nunc urna, rutrum sed quam non, consectetur lobortis leo. Vestibulum pretium turpis et hendrerit lacinia. Donec tempus fermentum vulputate. Phasellus at consequat libero. Etiam tincidunt, libero eu sollicitudin volutpat, ex nulla cursus libero, in mattis leo nisi id nulla. Nam quis tincidunt purus. Ut fringilla velit risus, sed iaculis velit laoreet quis. Etiam hendrerit a dui quis convallis. Suspendisse potenti. Integer bibendum venenatis neque, at vulputate risus varius sit amet. Maecenas tempor sed tortor ac egestas. Vivamus et interdum tortor, ac accumsan tortor.</p>" +
                        "<p>Aenean dictum scelerisque efficitur. Vestibulum finibus sem vitae sapien tristique egestas. Vestibulum ac molestie nunc. Morbi eget sagittis lacus, eu eleifend ipsum. Vestibulum tincidunt lectus odio. Donec convallis interdum lorem, quis lacinia augue porta vitae. Proin eros quam, condimentum ac lacinia sit amet, interdum vehicula eros. Donec placerat est ac gravida interdum.</p>" +
                        "<p>Suspendisse potenti. Pellentesque quis mollis sapien. Nunc euismod enim nec tellus interdum, nec fringilla tortor ultrices. Nulla fringilla vitae ex ac congue. Aenean tincidunt lectus id augue auctor, a aliquam lectus pharetra. Aenean vitae arcu nibh. Mauris eget tortor a massa malesuada egestas. Pellentesque dapibus nisi at elit fringilla, ut venenatis orci pulvinar. Aenean suscipit imperdiet molestie. Nam metus lectus, hendrerit vitae facilisis vitae, aliquet ac magna. Praesent ac congue nisi. Curabitur maximus metus justo, sit amet tempus diam tristique in. Vestibulum ut justo facilisis, venenatis sapien vel, tincidunt ante.</p>" +
                        "<p>Maecenas in enim nulla. Nulla nulla magna, hendrerit vel facilisis nec, mattis nec quam. In quis massa auctor, iaculis risus congue, mattis justo. Etiam ultrices purus ut ultrices auctor. Etiam bibendum quis justo vel congue. Mauris nec semper justo. Vestibulum dapibus vehicula justo, eu porttitor massa efficitur sit amet. Donec finibus libero a justo vestibulum tristique. Nulla eget metus metus. Sed eget blandit justo. Fusce ac nibh at risus posuere pretium. Aliquam id eleifend tellus.</p>" +
                        "<p>Fusce tellus dolor, sagittis rutrum odio a, vehicula eleifend lorem. Maecenas lacinia interdum malesuada. Cras dapibus felis enim, et tristique sem mollis a. Donec suscipit, arcu a maximus feugiat, nulla ipsum iaculis nulla, vel feugiat urna massa eu quam. Duis dignissim condimentum ex, sit amet pretium metus maximus eget. Suspendisse metus nisi, eleifend at tempor nec, semper et ipsum. Proin vitae sapien nec quam varius congue vitae fermentum elit. Curabitur sed lobortis odio. Phasellus semper porta ligula eu molestie. Aenean fermentum nisl blandit, bibendum arcu et, gravida purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce sed blandit felis. Mauris non lectus efficitur, efficitur erat vitae, viverra ipsum.</p>"

                    };

                    um.Create(author1, "author123");
                    um.AddToRole(author1.Id, "Author");
                    um.AddToRole(author1.Id, "User");
                    um.AddToRole(author1.Id, "Blogger");
                    um.AddToRole(author1.Id, "Reviewer");



                    var author2 = new Author
                    {
                        UserName = "reviewer@ltc.co.uk",
                        Email = "reviewer@ltc.co.uk",
                        FirstName = "Timothy",
                        LastName = "Smith",
                        JoinDate = DateTime.Now.AddYears(-2),
                        StaffSince = DateTime.Now.AddYears(-2),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        UserImage = "/Content/Images/Users/Profiles/Demo/reviewer.jpg",
                        Badges = new List<Badge> { reviewerBadge },
                        Biography = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla nec ex mi. Aliquam quis sapien a massa cursus euismod. Maecenas malesuada mauris tincidunt, fermentum libero et, vehicula nunc. Suspendisse quis feugiat ante. Aliquam nunc urna, rutrum sed quam non, consectetur lobortis leo. Vestibulum pretium turpis et hendrerit lacinia. Donec tempus fermentum vulputate. Phasellus at consequat libero. Etiam tincidunt, libero eu sollicitudin volutpat, ex nulla cursus libero, in mattis leo nisi id nulla. Nam quis tincidunt purus. Ut fringilla velit risus, sed iaculis velit laoreet quis. Etiam hendrerit a dui quis convallis. Suspendisse potenti. Integer bibendum venenatis neque, at vulputate risus varius sit amet. Maecenas tempor sed tortor ac egestas. Vivamus et interdum tortor, ac accumsan tortor.</p>" +
                        "<p>Aenean dictum scelerisque efficitur. Vestibulum finibus sem vitae sapien tristique egestas. Vestibulum ac molestie nunc. Morbi eget sagittis lacus, eu eleifend ipsum. Vestibulum tincidunt lectus odio. Donec convallis interdum lorem, quis lacinia augue porta vitae. Proin eros quam, condimentum ac lacinia sit amet, interdum vehicula eros. Donec placerat est ac gravida interdum.</p>" +
                        "<p>Suspendisse potenti. Pellentesque quis mollis sapien. Nunc euismod enim nec tellus interdum, nec fringilla tortor ultrices. Nulla fringilla vitae ex ac congue. Aenean tincidunt lectus id augue auctor, a aliquam lectus pharetra. Aenean vitae arcu nibh. Mauris eget tortor a massa malesuada egestas. Pellentesque dapibus nisi at elit fringilla, ut venenatis orci pulvinar. Aenean suscipit imperdiet molestie. Nam metus lectus, hendrerit vitae facilisis vitae, aliquet ac magna. Praesent ac congue nisi. Curabitur maximus metus justo, sit amet tempus diam tristique in. Vestibulum ut justo facilisis, venenatis sapien vel, tincidunt ante.</p>" +
                        "<p>Maecenas in enim nulla. Nulla nulla magna, hendrerit vel facilisis nec, mattis nec quam. In quis massa auctor, iaculis risus congue, mattis justo. Etiam ultrices purus ut ultrices auctor. Etiam bibendum quis justo vel congue. Mauris nec semper justo. Vestibulum dapibus vehicula justo, eu porttitor massa efficitur sit amet. Donec finibus libero a justo vestibulum tristique. Nulla eget metus metus. Sed eget blandit justo. Fusce ac nibh at risus posuere pretium. Aliquam id eleifend tellus.</p>" +
                        "<p>Fusce tellus dolor, sagittis rutrum odio a, vehicula eleifend lorem. Maecenas lacinia interdum malesuada. Cras dapibus felis enim, et tristique sem mollis a. Donec suscipit, arcu a maximus feugiat, nulla ipsum iaculis nulla, vel feugiat urna massa eu quam. Duis dignissim condimentum ex, sit amet pretium metus maximus eget. Suspendisse metus nisi, eleifend at tempor nec, semper et ipsum. Proin vitae sapien nec quam varius congue vitae fermentum elit. Curabitur sed lobortis odio. Phasellus semper porta ligula eu molestie. Aenean fermentum nisl blandit, bibendum arcu et, gravida purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce sed blandit felis. Mauris non lectus efficitur, efficitur erat vitae, viverra ipsum.</p>"

                    };

                    um.Create(author2, "reviewer123");
                    um.AddToRole(author2.Id, "Reviewer");
                    um.AddToRole(author2.Id, "User");
                    um.AddToRole(author2.Id, "Author");


                    var blogger = new Author
                    {
                        UserName = "blogger@ltc.co.uk",
                        Email = "blogger@ltc.co.uk",
                        FirstName = "Charlotte",
                        LastName = "Chiles",
                        JoinDate = DateTime.Now.AddYears(-1),
                        StaffSince = DateTime.Now.AddYears(-1),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        UserImage = "/Content/Images/Users/Profiles/Demo/blogger.jpg",
                        Badges = new List<Badge> { bloggerBadge },
                        Biography = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla nec ex mi. Aliquam quis sapien a massa cursus euismod. Maecenas malesuada mauris tincidunt, fermentum libero et, vehicula nunc. Suspendisse quis feugiat ante. Aliquam nunc urna, rutrum sed quam non, consectetur lobortis leo. Vestibulum pretium turpis et hendrerit lacinia. Donec tempus fermentum vulputate. Phasellus at consequat libero. Etiam tincidunt, libero eu sollicitudin volutpat, ex nulla cursus libero, in mattis leo nisi id nulla. Nam quis tincidunt purus. Ut fringilla velit risus, sed iaculis velit laoreet quis. Etiam hendrerit a dui quis convallis. Suspendisse potenti. Integer bibendum venenatis neque, at vulputate risus varius sit amet. Maecenas tempor sed tortor ac egestas. Vivamus et interdum tortor, ac accumsan tortor.</p>" +
                        "<p>Aenean dictum scelerisque efficitur. Vestibulum finibus sem vitae sapien tristique egestas. Vestibulum ac molestie nunc. Morbi eget sagittis lacus, eu eleifend ipsum. Vestibulum tincidunt lectus odio. Donec convallis interdum lorem, quis lacinia augue porta vitae. Proin eros quam, condimentum ac lacinia sit amet, interdum vehicula eros. Donec placerat est ac gravida interdum.</p>" +
                        "<p>Suspendisse potenti. Pellentesque quis mollis sapien. Nunc euismod enim nec tellus interdum, nec fringilla tortor ultrices. Nulla fringilla vitae ex ac congue. Aenean tincidunt lectus id augue auctor, a aliquam lectus pharetra. Aenean vitae arcu nibh. Mauris eget tortor a massa malesuada egestas. Pellentesque dapibus nisi at elit fringilla, ut venenatis orci pulvinar. Aenean suscipit imperdiet molestie. Nam metus lectus, hendrerit vitae facilisis vitae, aliquet ac magna. Praesent ac congue nisi. Curabitur maximus metus justo, sit amet tempus diam tristique in. Vestibulum ut justo facilisis, venenatis sapien vel, tincidunt ante.</p>" +
                        "<p>Maecenas in enim nulla. Nulla nulla magna, hendrerit vel facilisis nec, mattis nec quam. In quis massa auctor, iaculis risus congue, mattis justo. Etiam ultrices purus ut ultrices auctor. Etiam bibendum quis justo vel congue. Mauris nec semper justo. Vestibulum dapibus vehicula justo, eu porttitor massa efficitur sit amet. Donec finibus libero a justo vestibulum tristique. Nulla eget metus metus. Sed eget blandit justo. Fusce ac nibh at risus posuere pretium. Aliquam id eleifend tellus.</p>" +
                        "<p>Fusce tellus dolor, sagittis rutrum odio a, vehicula eleifend lorem. Maecenas lacinia interdum malesuada. Cras dapibus felis enim, et tristique sem mollis a. Donec suscipit, arcu a maximus feugiat, nulla ipsum iaculis nulla, vel feugiat urna massa eu quam. Duis dignissim condimentum ex, sit amet pretium metus maximus eget. Suspendisse metus nisi, eleifend at tempor nec, semper et ipsum. Proin vitae sapien nec quam varius congue vitae fermentum elit. Curabitur sed lobortis odio. Phasellus semper porta ligula eu molestie. Aenean fermentum nisl blandit, bibendum arcu et, gravida purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce sed blandit felis. Mauris non lectus efficitur, efficitur erat vitae, viverra ipsum.</p>"

                    };

                    um.Create(blogger, "blogger123");
                    um.AddToRole(blogger.Id, "Blogger");
                    um.AddToRole(blogger.Id, "User");
                    um.AddToRole(blogger.Id, "Author");



                    var author4 = new Author
                    {
                        UserName = "reporter@ltc.co.uk",
                        Email = "reporter@ltc.co.uk",
                        FirstName = "Ania",
                        LastName = "Bell",
                        JoinDate = DateTime.Now.AddYears(-1),
                        StaffSince = DateTime.Now.AddYears(-1),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        UserImage = "/content/images/users/profiles/demo/default.png",
                        Badges = new List<Badge> { reporterBadge },
                        Biography = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla nec ex mi. Aliquam quis sapien a massa cursus euismod. Maecenas malesuada mauris tincidunt, fermentum libero et, vehicula nunc. Suspendisse quis feugiat ante. Aliquam nunc urna, rutrum sed quam non, consectetur lobortis leo. Vestibulum pretium turpis et hendrerit lacinia. Donec tempus fermentum vulputate. Phasellus at consequat libero. Etiam tincidunt, libero eu sollicitudin volutpat, ex nulla cursus libero, in mattis leo nisi id nulla. Nam quis tincidunt purus. Ut fringilla velit risus, sed iaculis velit laoreet quis. Etiam hendrerit a dui quis convallis. Suspendisse potenti. Integer bibendum venenatis neque, at vulputate risus varius sit amet. Maecenas tempor sed tortor ac egestas. Vivamus et interdum tortor, ac accumsan tortor.</p>" +
                        "<p>Aenean dictum scelerisque efficitur. Vestibulum finibus sem vitae sapien tristique egestas. Vestibulum ac molestie nunc. Morbi eget sagittis lacus, eu eleifend ipsum. Vestibulum tincidunt lectus odio. Donec convallis interdum lorem, quis lacinia augue porta vitae. Proin eros quam, condimentum ac lacinia sit amet, interdum vehicula eros. Donec placerat est ac gravida interdum.</p>" +
                        "<p>Suspendisse potenti. Pellentesque quis mollis sapien. Nunc euismod enim nec tellus interdum, nec fringilla tortor ultrices. Nulla fringilla vitae ex ac congue. Aenean tincidunt lectus id augue auctor, a aliquam lectus pharetra. Aenean vitae arcu nibh. Mauris eget tortor a massa malesuada egestas. Pellentesque dapibus nisi at elit fringilla, ut venenatis orci pulvinar. Aenean suscipit imperdiet molestie. Nam metus lectus, hendrerit vitae facilisis vitae, aliquet ac magna. Praesent ac congue nisi. Curabitur maximus metus justo, sit amet tempus diam tristique in. Vestibulum ut justo facilisis, venenatis sapien vel, tincidunt ante.</p>" +
                        "<p>Maecenas in enim nulla. Nulla nulla magna, hendrerit vel facilisis nec, mattis nec quam. In quis massa auctor, iaculis risus congue, mattis justo. Etiam ultrices purus ut ultrices auctor. Etiam bibendum quis justo vel congue. Mauris nec semper justo. Vestibulum dapibus vehicula justo, eu porttitor massa efficitur sit amet. Donec finibus libero a justo vestibulum tristique. Nulla eget metus metus. Sed eget blandit justo. Fusce ac nibh at risus posuere pretium. Aliquam id eleifend tellus.</p>" +
                        "<p>Fusce tellus dolor, sagittis rutrum odio a, vehicula eleifend lorem. Maecenas lacinia interdum malesuada. Cras dapibus felis enim, et tristique sem mollis a. Donec suscipit, arcu a maximus feugiat, nulla ipsum iaculis nulla, vel feugiat urna massa eu quam. Duis dignissim condimentum ex, sit amet pretium metus maximus eget. Suspendisse metus nisi, eleifend at tempor nec, semper et ipsum. Proin vitae sapien nec quam varius congue vitae fermentum elit. Curabitur sed lobortis odio. Phasellus semper porta ligula eu molestie. Aenean fermentum nisl blandit, bibendum arcu et, gravida purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce sed blandit felis. Mauris non lectus efficitur, efficitur erat vitae, viverra ipsum.</p>"

                    };

                    um.Create(author4, "reporter123");
                    um.AddToRole(author4.Id, "Author");
                    um.AddToRole(author4.Id, "Reporter");
                    um.AddToRole(author4.Id, "User");

                    //EDITOR!
                    var editor = new Author
                    {
                        UserName = "editor@ltc.co.uk",
                        Email = "editor@ltc.co.uk",
                        FirstName = "George",
                        LastName = "McKwoski",
                        JoinDate = DateTime.Now.AddMonths(-1),
                        StaffSince = DateTime.Now.AddDays(-29),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        Badges = new List<Badge> { reporterBadge },
                        UserImage = "/content/images/users/profiles/demo/default.png",
                        Biography = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla nec ex mi. Aliquam quis sapien a massa cursus euismod. Maecenas malesuada mauris tincidunt, fermentum libero et, vehicula nunc. Suspendisse quis feugiat ante. Aliquam nunc urna, rutrum sed quam non, consectetur lobortis leo. Vestibulum pretium turpis et hendrerit lacinia. Donec tempus fermentum vulputate. Phasellus at consequat libero. Etiam tincidunt, libero eu sollicitudin volutpat, ex nulla cursus libero, in mattis leo nisi id nulla. Nam quis tincidunt purus. Ut fringilla velit risus, sed iaculis velit laoreet quis. Etiam hendrerit a dui quis convallis. Suspendisse potenti. Integer bibendum venenatis neque, at vulputate risus varius sit amet. Maecenas tempor sed tortor ac egestas. Vivamus et interdum tortor, ac accumsan tortor.</p>" +
                        "<p>Aenean dictum scelerisque efficitur. Vestibulum finibus sem vitae sapien tristique egestas. Vestibulum ac molestie nunc. Morbi eget sagittis lacus, eu eleifend ipsum. Vestibulum tincidunt lectus odio. Donec convallis interdum lorem, quis lacinia augue porta vitae. Proin eros quam, condimentum ac lacinia sit amet, interdum vehicula eros. Donec placerat est ac gravida interdum.</p>" +
                        "<p>Suspendisse potenti. Pellentesque quis mollis sapien. Nunc euismod enim nec tellus interdum, nec fringilla tortor ultrices. Nulla fringilla vitae ex ac congue. Aenean tincidunt lectus id augue auctor, a aliquam lectus pharetra. Aenean vitae arcu nibh. Mauris eget tortor a massa malesuada egestas. Pellentesque dapibus nisi at elit fringilla, ut venenatis orci pulvinar. Aenean suscipit imperdiet molestie. Nam metus lectus, hendrerit vitae facilisis vitae, aliquet ac magna. Praesent ac congue nisi. Curabitur maximus metus justo, sit amet tempus diam tristique in. Vestibulum ut justo facilisis, venenatis sapien vel, tincidunt ante.</p>" +
                        "<p>Maecenas in enim nulla. Nulla nulla magna, hendrerit vel facilisis nec, mattis nec quam. In quis massa auctor, iaculis risus congue, mattis justo. Etiam ultrices purus ut ultrices auctor. Etiam bibendum quis justo vel congue. Mauris nec semper justo. Vestibulum dapibus vehicula justo, eu porttitor massa efficitur sit amet. Donec finibus libero a justo vestibulum tristique. Nulla eget metus metus. Sed eget blandit justo. Fusce ac nibh at risus posuere pretium. Aliquam id eleifend tellus.</p>" +
                        "<p>Fusce tellus dolor, sagittis rutrum odio a, vehicula eleifend lorem. Maecenas lacinia interdum malesuada. Cras dapibus felis enim, et tristique sem mollis a. Donec suscipit, arcu a maximus feugiat, nulla ipsum iaculis nulla, vel feugiat urna massa eu quam. Duis dignissim condimentum ex, sit amet pretium metus maximus eget. Suspendisse metus nisi, eleifend at tempor nec, semper et ipsum. Proin vitae sapien nec quam varius congue vitae fermentum elit. Curabitur sed lobortis odio. Phasellus semper porta ligula eu molestie. Aenean fermentum nisl blandit, bibendum arcu et, gravida purus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce sed blandit felis. Mauris non lectus efficitur, efficitur erat vitae, viverra ipsum.</p>"

                    };

                    um.Create(editor, "editor123");
                    um.AddToRole(editor.Id, "User");
                    um.AddToRole(editor.Id, "Editor");

                    //USERS
                    var user1 = new User
                    {
                        UserName = "user@ltc.co.uk",
                        Email = "user@ltc.co.uk",
                        FirstName = "Ryan",
                        LastName = "Toal",
                        JoinDate = DateTime.Now.AddMonths(-7),
                        EmailConfirmed = true,
                        NumberOfWarnings = 0,
                        CommentsBlocked = false,
                        AccountSuspended = false,
                        Badges = new List<Badge> (),
                        UserImage = "/content/images/users/profiles/demo/default.png",
                    };

                    um.Create(user1, "user123");
                    um.AddToRole(user1.Id, "User");


                    var user2 = new User
                    {
                        UserName = "user2@ltc.co.uk",
                        Email = "user2@ltc.co.uk",
                        FirstName = "Stephen",
                        LastName = "Scott",
                        JoinDate = DateTime.Now.AddMonths(-5),
                        EmailConfirmed = true,
                        NumberOfWarnings = 3,
                        CommentsBlocked = true,
                        AccountSuspended = false,
                        Badges = new List<Badge>(),
                        UserImage = "/content/images/users/profiles/demo/default.png",
                    };

                    um.Create(user2, "user321");
                    um.AddToRole(user2.Id, "User");

                    var user3 = new User
                    {
                        UserName = "usersus@ltc.co.uk",
                        Email = "usersus@ltc.co.uk",
                        FirstName = "Matthew",
                        LastName = "Pilkenton",
                        JoinDate = DateTime.Now.AddMonths(-10),
                        EmailConfirmed = true,
                        NumberOfWarnings = 12,
                        CommentsBlocked = true,
                        CommentsBlockedUntil = DateTime.Now.AddDays(5),
                        Badges = new List<Badge>(),
                        UserImage = "/content/images/users/profiles/demo/default.png",

                        AccountSuspended = true,
                        SuspensionUntil = DateTime.Now.AddDays(1)
                    };

                    um.Create(user3, "usersus");
                    um.AddToRoles(user3.Id, "User");
                    um.AddToRoles(user3.Id, "Suspended");
                    um.AddToRoles(user3.Id, "Silenced");


                    var user4 = new User
                    {
                        UserName = "userblock@ltc.co.uk",
                        Email = "userblock@ltc.co.uk",
                        FirstName = "Dana",
                        LastName = "Legend",
                        JoinDate = DateTime.Now.AddMonths(-9),
                        EmailConfirmed = true,
                        NumberOfWarnings = 3,
                        Badges = new List<Badge>(),
                        UserImage = "/content/images/users/profiles/demo/default.png",

                        CommentsBlocked = true,
                        CommentsBlockedUntil = DateTime.Now.AddDays(5),

                        AccountSuspended = false
                    };

                    um.Create(user4, "userblock");
                    um.AddToRoles(user4.Id, "User");
                    um.AddToRoles(user4.Id, "Silenced");


                    context.SaveChanges();



                    //categories and genres

                    var cat1 = new Category
                    {
                        Name = "Film"
                    };
                    var cat2 = new Category
                    {
                        Name = "TV"
                    };
                    var cat3 = new Category
                    {
                        Name = "Streaming"
                    };
                    var cat4 = new Category
                    {
                        Name = "Cinemas"
                    };
                    var cat5 = new Category
                    {
                        Name = "Theatre"
                    };
                    var cat6 = new Category
                    {
                        Name = "Plays"
                    };

                    context.Categories.Add(cat1);
                    context.Categories.Add(cat2);
                    context.Categories.Add(cat3);
                    context.Categories.Add(cat4);
                    context.Categories.Add(cat5);
                    context.Categories.Add(cat6);

                    context.SaveChanges();


                    //genres
                    var horror = new Genre
                    {
                        Name = "Horror"
                    };
                    var action = new Genre
                    {
                        Name = "Action"
                    };
                    var romance = new Genre
                    {
                        Name = "Romance"
                    };
                    var scifi = new Genre
                    {
                        Name = "Sci-fi"
                    };
                    var thirller = new Genre
                    {
                        Name = "Thriller"
                    };
                    var drama = new Genre
                    {
                        Name = "Drama"
                    };
                    var comedy = new Genre
                    {
                        Name = "Comedy"
                    };
                    var animation = new Genre
                    {
                        Name = "Animation"
                    };
                    var documentary = new Genre
                    {
                        Name = "Documentary"
                    };

                    context.Genres.Add(horror);
                    context.Genres.Add(action);
                    context.Genres.Add(romance);
                    context.Genres.Add(scifi);
                    context.Genres.Add(thirller);
                    context.Genres.Add(drama);
                    context.Genres.Add(comedy);
                    context.Genres.Add(animation);

                    context.SaveChanges();

                    var film = new ReviewCategory
                    {
                        Name = "Film"
                    };
                    var tv = new ReviewCategory
                    {
                        Name = "TV"
                    };
                    var indie = new ReviewCategory
                    {
                        Name = "Indie"
                    };
                    var theatre = new ReviewCategory
                    {
                        Name = "Theatre"
                    };
                    

                    context.ReviewCategories.Add(film);
                    context.ReviewCategories.Add(tv);
                    context.ReviewCategories.Add(indie);
                    context.ReviewCategories.Add(theatre);

                    context.SaveChanges();

                    //REPORT REASONS
                    var rr1 = new ReportReason
                    {
                        Reason = "Innapropriate"
                    };
                    var rr2 = new ReportReason
                    {
                        Reason = "Harrasment"
                    };
                    var rr3 = new ReportReason
                    {
                        Reason = "Abusive"
                    };
                    var rr4 = new ReportReason
                    {
                        Reason = "Spamming"
                    };
                    var rr5 = new ReportReason
                    {
                        Reason = "Other"
                    };

                    context.ReportReasons.Add(rr1);
                    context.ReportReasons.Add(rr2);
                    context.ReportReasons.Add(rr3);
                    context.ReportReasons.Add(rr4);
                    context.ReportReasons.Add(rr5);

                    context.SaveChanges();

                    //BANNED STUFF
                    context.BannedContents.Add(new BannedContent
                    {
                        Content = "fuck"
                    });
                    context.BannedContents.Add(new BannedContent
                    {
                        Content = "shit"
                    });
                    context.BannedContents.Add(new BannedContent
                    {
                        Content = "cunt"
                    });
                    context.BannedContents.Add(new BannedContent
                    {
                        Content = "twat"
                    });
                    context.BannedContents.Add(new BannedContent
                    {
                        Content = "hitler was right"
                    });

                    //POSTS!
                    //REVIEWS
                    var review1 = new Review()
                    {
                        Title = "Land (2021)",
                        Synopsis = "Following an unnamed heartbreak, Edee (Robin Wright) leaves her city life behind to live off-grid in a cabin high in the Wyoming Rockies. Lacking survival skills, she comes perilously close to death before a local hunter (Demián Bichir) saves her life and the two slowly form a precious bond.",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "Omg so pog",
                                User = admin,
                                DatePosted = DateTime.Now.AddDays(-2),
                                //EditedAt = DateTime.Now.AddDays(-2),
                                Votes = new List<CommentVote>{ 
                                    new CommentVote { User = user1,UpVote = true },
                                    new CommentVote { User = user2,UpVote = true },
                                    new CommentVote { User = user4,UpVote = true },
                                    new CommentVote { User = author1,UpVote = true },
                                    new CommentVote { User = user3,UpVote = false },
                                },
                                Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "MORE COMMENTS!",
                                                        User = author1,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                            },
                                            new Comment
                                            {
                                                Content = "WHATYA GOIng to do about it",
                                                User = author2,
                                                DatePosted = DateTime.Now.AddDays(-1),
                                            }
                                        }
                            },
                            new Comment
                            {
                                Content = "Wow I loved this film, adn you slated it!",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Content = "I agree, Sir Anothony Hopkins did a great job",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                        Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "DELETED",
                                                User = author4,
                                                DatePosted = DateTime.Now.AddDays(-1),
                                                DELTED = true,
                                                Replies = new List<Comment>
                                                {
                                                    new Comment{
                                                        Content = "Omg what was this comment?!",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Comment
                                    {
                                        Content = "sploge mc duck",
                                        User = user4,
                                        DatePosted = DateTime.Now.AddDays(-1).AddHours(-5),
                                    }

                                }
                            }
                        },
                        DatePosted = DateTime.Now.AddDays(-171),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            drama,comedy
                        },
                        Score = 2.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/land.jpg",
                        Draft = false,
                        Published = true,
                        Votes = new List<PostVote> { new PostVote { UpVote = true, User = user1}, new PostVote { UpVote = true, User = admin } }
                    };

                    context.Posts.Add(review1);
                    context.SaveChanges();
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///
                    var review2 = new Review()
                    {
                        Title = "A Quiet Place Part II",
                        Synopsis = "Day 474 of the alien attack. The Abbott family leave their farm house and venture into the outside world beyond the sand path. But now it’s not just those pesky critters who stalk by sound that they have to evade",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<h4>The Twist</h4>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {
                            new Comment
                            {
                                Content = "Omg so pog",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "MORE COMMENTS!",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                            },
                                            new Comment
                                            {
                                                Content = "WHATYA GOIng to do about it",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1)
                                            },
                                            new Comment
                                            {
                                                Content = "Moire Comments!",
                                                        User = author4,
                                                        DatePosted = DateTime.Now.AddDays(-1)
                                            }
                                        }
                            },
                            new Comment
                            {
                                Content = "Wow I loved this film, adn you slated it!",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Content = "I agree, Sir Anothony Hopkins did a great job",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                        Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "fucking working",
                                                User = user3,
                                                DatePosted = DateTime.Now.AddDays(-1),
                                                Replies = new List<Comment>
                                                {
                                                    new Comment{
                                                        Content = "INCEPTION BITCH!",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Comment
                                    {
                                        Content = "sploge mc duck",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                    }

                                }
                            },
                            new Comment
                            {
                                Content = "Why dont you talk about amanda being gud?!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-2),
                            }

                        },
                        DatePosted = DateTime.Now.AddDays(-29),
                        PostLocked = true,
                        Genres = new List<Genre>
                        {
                            scifi
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            film
                        },
                        Score = 2.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/quiet.jpg",
                        Draft = false,
                        Published = true,
                        Votes = new List<PostVote> { new PostVote { UpVote = true, User = user1 }, new PostVote { UpVote = true, User = admin }, new PostVote { UpVote = true,User = user2 }, new PostVote { UpVote = true,User = user3 } }
                    };

                    context.Posts.Add(review2);
                    context.SaveChanges();

                    var review3 = new Review()
                    {
                        Title = "Hotfuzz 2",
                        Synopsis = "The cornetto trilogy returns with a bang!",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "Great review",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                            },
                            new Comment
                            {
                                Content = "Wow I loved this film, adn you slated it!",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Content = "I agree, Sir Anothony Hopkins did a great job",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                    },
                                    new Comment
                                    {
                                        Content = "sploge mc duck",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1).AddHours(-5),
                                    }

                                }
                            },
                            new Comment
                            {
                                Content = "Loved it!",
                                        User = user2,
                                        DatePosted = DateTime.Now.AddDays(-3).AddHours(-10),
                            }
                        },
                        DatePosted = DateTime.Now.AddDays(1),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            action,comedy
                        },
                        Score = 2.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/hotfuzz.jpg",
                        Draft = false,
                        Published = true,
                        Votes = new List<PostVote> { new PostVote { UpVote = true, User = user1 }, new PostVote { UpVote = true, User = admin }, new PostVote { UpVote = true, User = user2 }, new PostVote { UpVote = true, User = user3 } }
                    };

                    context.Posts.Add(review3);
                    context.SaveChanges();

                    var review4 = new Review()
                    {
                        Votes = new List<PostVote> { new PostVote { UpVote = true, User = user1 }, new PostVote { UpVote = true, User = admin }, new PostVote { UpVote = true, User = user2 }, new PostVote { UpVote = true, User = user3 }, new PostVote { User = user3, UpVote = false } },
                        Title = "Wicked: COVID Viewing",
                        Synopsis = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<h4>The Twist</h4>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {
                            new Comment
                            {
                                Content = "Omg so pog",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "MORE COMMENTS!",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                            },
                                            new Comment
                                            {
                                                Content = "WHATYA GOIng to do about it",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1)
                                            },
                                            new Comment
                                            {
                                                Content = "Moire Comments!",
                                                        User = author4,
                                                        DatePosted = DateTime.Now.AddDays(-1)
                                            }
                                        }
                            },
                            new Comment
                            {
                                Content = "Wow I loved this film, adn you slated it!",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Content = "I agree, Sir Anothony Hopkins did a great job",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                        Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "fucking working",
                                                User = user3,
                                                DatePosted = DateTime.Now.AddDays(-1),
                                                Replies = new List<Comment>
                                                {
                                                    new Comment{
                                                        Content = "INCEPTION BITCH!",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Comment
                                    {
                                        Content = "sploge mc duck",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                    }

                                }
                            },
                            new Comment
                            {
                                Content = "Why dont you talk about amanda being gud?!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-2),
                            }

                        },
                        DatePosted = DateTime.Now.AddDays(-29),
                        PostLocked = true,
                        Genres = new List<Genre>
                        {
                            horror
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            theatre
                        },
                        Score = 4M,
                        FeatureImagePath = "/Content/Images/Posts/Review/wicked.jpg",
                        Draft = false,
                        Published = true

                    };

                    context.Posts.Add(review4);
                    context.SaveChanges();

                    var review5 = new Review()
                    {
                        Votes = new List<PostVote> { new PostVote { UpVote = true, User = user1 }, new PostVote { UpVote = true, User = admin }, new PostVote { UpVote = true, User = user2 }, new PostVote { UpVote = true, User = user3 } },
                        Title = "Godzilla vs. Kong",
                        Synopsis = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum.",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "Such a great film, and a great review!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "Thank you, I'm glad you enjoyed both the film, and my review!",
                                                        User = author2,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                            }
                                        }
                            },
                            new Comment
                            {
                                Content = "Wow I loved this film, adn you slated it!",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Content = "I think Mechazilla should have won",
                                        User = user3,
                                        DatePosted = DateTime.Now.AddDays(-1),
                                        Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "Well you are entitled to your opinion!",
                                                User = author2,
                                                DatePosted = DateTime.Now.AddHours(-5),
                                                Replies = new List<Comment>
                                                {
                                                    new Comment{
                                                        Content = "Thanks!",
                                                        User = user3,
                                                        DatePosted = DateTime.Now.AddHours(-2),
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        },
                        DatePosted = DateTime.Now.AddMonths(-2),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            action,thirller
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/godzilla.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review5);
                    context.SaveChanges();

                    var review6 = new Review()
                    {
                        Title = "The 8th",
                        Synopsis = "A documentary of the run-up to the 2018 referendum in Ireland to repeal the 8th amendment, granting women abortion rights. Although embedded within the ‘YES’ campaign, it features passionate interviewees from both sides, painting a vivid picture of social battle lines.",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "Such a great film, and a great review!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "Thank you, I'm glad you enjoyed both the film, and my review!",
                                                        User = author2,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                            }
                                        }
                            }
                        },
                        DatePosted = DateTime.Now.AddMonths(-3),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            documentary
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            indie
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/8th.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review6);
                    context.SaveChanges();

                    var review7 = new Review()
                    {
                        Votes = new List<PostVote> { new PostVote { UpVote = false, User = user1 }, new PostVote { UpVote = false, User = admin }, new PostVote { UpVote = false, User = user2 }, new PostVote { UpVote = true, User = user3 } },
                        Title = "Sonita",
                        Synopsis = "When Sonita premiered last year at Amsterdam’s documentary film festival IDFA, it walked away with the audience award, a win that isn’t too surprising considering the film’s story. ",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        
                        DatePosted = DateTime.Now.AddMonths(-3),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            drama
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            indie
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/sonita.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review7);
                    context.SaveChanges();

                    var review8 = new Review()
                    {
                        Title = "Lisey's Story",
                        Synopsis = "The widow of celebrated author Scott Landon (Clive Owen), Lisey Landon (Julianne Moore) becomes the focus for an obsessed stalker, Jim Dooley (Dane DeHaan), who wants to bring Scott’s unpublished works into the world. But revisiting Scott’s manuscripts prompts Lisey to consider her husband’s dark past. ",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                       "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                       "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",

                        DatePosted = DateTime.Now.AddMonths(-3),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            drama, romance
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            tv
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/liseys.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review8);
                    context.SaveChanges();

                    var review9 = new Review()
                    {
                        Title = "Fargo Season 4",
                        Synopsis = "The 1950s. Kansas City crime bosses Josto Fadda (Jason Schwartzman) and Loy Cannon (Chris Rock) look set for gang warfare, nurse Oraetta Mayflower (Jessie Buckley) goes on a killing spree, and lawmen Dick Wickware (Timothy Olyphant) and Odis Weff (Jack Huston) chase two escaped cons.",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                       "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                       "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",

                        DatePosted = DateTime.Now.AddMonths(-5),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            action, drama, 
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            tv
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/fargo.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review9);
                    context.SaveChanges();

                    var review10 = new Review()
                    {
                        Title = "Line of Duty Series 6",
                        Synopsis = "A new case comes AC-12’s way after a murder investigation unit led by boss DCI Joanne Davidson (Kelly Macdonald) bungle a high-profile job, letting a likely murder suspect escape. But with Kate (Vicky McClure) having left the anti-corruption team and now part of the one under scrutiny, will she work with her old colleagues or against them? Plus, will we ever, for the love of God, find out who H is?",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                      "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                      "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                      "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                      "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",

                        DatePosted = DateTime.Now,
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                            action, drama,
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            tv
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/duty.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review10);
                    context.SaveChanges();

                    var review11 = new Review()
                    {
                        Title = "Your Honor",
                        Synopsis = "Adam Desiato (Hunter Doohan) kills another teenager in a hit-and-run incident and his father (Bryan Cranston), a New Orleans judge, convinces him to face the music. But when it becomes apparent the dead boy’s father is don to the city’s biggest crime family, the judge decides to risk everything and cover up the crime.",
                        Author = author2,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                      "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                      "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                      "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                      "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",

                        DatePosted = DateTime.Now.AddDays(-25),
                        PostLocked = false,
                        Genres = new List<Genre>
                        {
                             drama,thirller
                        },
                        ReviewCategories = new List<ReviewCategory>
                        {
                            tv
                        },
                        Score = 3.5M,
                        FeatureImagePath = "/Content/Images/Posts/Review/honor.jpg",
                        Draft = false,
                        Published = true
                    };
                    context.Posts.Add(review11);
                    context.SaveChanges();

                    var reviewDraft = new Review()
                    {
                        Title = "Nomadlands",
                        Synopsis = "Sharron Stavovski potrays life in modern day America through the eyes of those with homes on wheels!",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Draft = true,
                        Published = false,
                        DatePosted = DateTime.Now.AddDays(-3),
                    };

                    context.Posts.Add(reviewDraft);
                    context.SaveChanges();

                    var reviewPending = new Review()
                    {
                        Title = "Spider-man: Into the Spiderverse",
                        Synopsis = "An animation with such style Harry Styles is just Harry s now!",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Draft = false,
                        Published = false,
                        ReviewCategories = new List<ReviewCategory> { film },
                        Genres = new List<Genre> { action, animation },
                        Editor = editor,
                        DatePosted = DateTime.Now.AddDays(-3),
                        Score = 4L,
                        Comments = new List<Comment>(),
                        FeatureImagePath = "/Content/Images/Posts/Review/spiderverse.jpg",
                        Reports = new List<Report>(),
                        Flagged = false,
                        Immune = false
                    };

                    context.Posts.Add(reviewPending);
                    context.SaveChanges();

                    //Blogs
                    var blog1 = new Blog
                    {
                        Title = "How fading actos saved their carears",
                        Synopsis = "Join me as I discuss how several child star rebuild their fame!",
                        DatePosted = DateTime.Now.AddDays(1),
                        Categories = new List<Category> { cat4, cat1},
                        FeatureImagePath = "/Content/Images/Posts/Blog/harrypotter.jpg",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "I agree, the way the movies is going is odd",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                                Replies = new List<Comment>
                                        {
                                            new Comment
                                            {
                                                Content = "MORE COMMENTS!",
                                                        User = author2,
                                                        DatePosted = DateTime.Now.AddDays(-1),
                                            },
                                            new Comment
                                            {
                                                Content = "WHATYA GOIng to do about it",
                                                User = user2,
                                                DatePosted = DateTime.Now.AddDays(-1),
                                            }
                                        }
                            },
                            new Comment
                            {
                                Content = "Wow I loved this film, adn you slated it!",
                                User = admin,
                                DatePosted = DateTime.Now.AddDays(-2),
                            }
                        },
                        Draft = false,
                        Published = true
                    };

                    var blog2 = new Blog
                    {
                        Title = "Movie Horror Tropes",
                        Synopsis = "Horror movies are great, but when do they get to much",
                        DatePosted = DateTime.Now.AddDays(-23),
                        Categories = new List<Category> { cat2, cat1 },
                        FeatureImagePath = "/Content/Images/Posts/Blog/horror.jpg",
                        Author = author1,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "I loved Derke Van Goobies in The Hackening",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                            }
                        },
                        Draft = false,
                        Published = true
                    };

                    var blog3 = new Blog
                    {
                        Title = "James Bond Producers Confirm They’re ‘Committed’ To 007 Remaining In Cinemas",
                        Synopsis = "The key word to note there is ‘theatrical’ – the intention here is for Bond to remain a big-screen player. There is, perhaps, leeway in the statement – if ‘James Bond films’ refers to the mainline series, there could be potential for Amazon to pursue spin-offs or ancillary adventures that wouldn’t necessarily be intended for cinema release, though there have been no rumours or plans as of yet for such projects.",
                        DatePosted = DateTime.Now.AddDays(-5),
                        Categories = new List<Category> {  cat1 },
                        FeatureImagePath = "/Content/Images/Posts/Blog/bond.jpg",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "Best James Bond EVER!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                            },
                            new Comment
                            {
                                Content = "I loved Derke Van Goobies in The Hackening",
                                User = user4,
                                DatePosted = DateTime.Now.AddDays(-2),
                            }
                        },
                        Draft = false,
                        Published = true
                    };

                    context.Posts.Add(blog3);
                    context.SaveChanges();

                    var blog4 = new Blog
                    {
                        Title = "Chris Pratt Is Drafted In The Trailer For The Tomorrow War",
                        Synopsis = "Thanks to Guardians Of The Galaxy, we're used to seeing Chris Pratt dealing with being transported somewhere he has to deal with aliens and conflicts. So he's on familiar – if slightly darker – territory here via Amazon sci-fi film The Tomorrow War.",
                        DatePosted = DateTime.Now.AddMonths(-2),
                        Categories = new List<Category> { cat1 },
                        FeatureImagePath = "/Content/Images/Posts/Blog/pratt.jpg",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "ah mah gahd, chris is soooo hawt",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-34),
                                Replies = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Content = "ohh 100%",
                                        User = user2,
                                        DatePosted = DateTime.Now.AddDays(-33),
                                    }
                                }
                            }
                        },
                        Draft = false,
                        Published = true
                    };

                    context.Posts.Add(blog4);
                    context.SaveChanges();

                    var blog5 = new Blog
                    {
                        Title = "Old Trailer: M Night Shyamalan’s Latest Brings Time-Ticking Terror To The Beach.",
                        Synopsis = "Beach horror tends to mean creature features – from shark movies like Jaws and The Shallows, to the underseen Blumhouse monster movie Sweetheart. But in M. Night Shyamalan’s next film, Old, the monster appears to be the ravages of time itself",
                        DatePosted = DateTime.Now.AddDays(-15),
                        Categories = new List<Category> { cat2 },
                        FeatureImagePath = "/Content/Images/Posts/Blog/shamy.jpg",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        
                        Draft = false,
                        Published = true
                    };

                    context.Posts.Add(blog5);
                    context.SaveChanges();

                    var blog6 = new Blog
                    {
                        Title = "Alyssa Sutherland And Lily Sullivan Starring In Evil Dead Rise",
                        Synopsis = "We've known for a while that original Evil Dead filmmaking trio Sam Raimi, Rob Tapert and Bruce Campbell were planning a new film in the franchise, setting up a brand new story set in the same deadite-afflicted world. The official details have arrived, with word that Alyssa Sutherland and Lily Sullivan will star in Evil Dead Rise",
                        DatePosted = DateTime.Now.AddDays(-1),
                        Categories = new List<Category> { cat3, cat6 },
                        FeatureImagePath = "/Content/Images/Posts/Blog/alyssa.jpg",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",

                        Draft = false,
                        Published = true
                    };

                    context.Posts.Add(blog6);
                    context.SaveChanges();

                    var blogDraft = new Blog()
                    {
                        Title = "Updates on the way LTC #15",
                        Synopsis = "",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                         "<p> nsequat id porta nibh venenatis cras sed felis eget.</p>",
                        Draft = true,
                        Published = false,
                        DatePosted = DateTime.Now.AddDays(-3),
                        Categories = new List<Category> { cat4}
                    };

                    context.Posts.Add(blogDraft);
                    context.SaveChanges();

                    var blogPending = new Blog()
                    {
                        Title = "Bees are finding a way into Hollywood",
                        Synopsis = "It may seem strange, but the plucky pollen collectors have quite a growing niche market.",
                        Author = blogger,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +                   
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Draft = false,
                        Published = false,
                        Categories = new List<Category> { cat1, cat2 },
                        Editor = editor,
                        DatePosted = DateTime.Now.AddDays(-2),
                        Comments = new List<Comment>(),
                        FeatureImagePath = "/Content/Images/Posts/Blog/bees.jpg",
                        Reports = new List<Report>(),
                        Flagged = false,
                        Immune = false
                    };

                    context.Posts.Add(blogPending);
                    context.SaveChanges();

                    context.Posts.Add(blog1);
                    context.Posts.Add(blog2);
                    context.SaveChanges();
                    //NEWS
                    var news1 = new News
                    {
                        Title = "The Flash: New change in director",
                        Synopsis = "Flash faces more changes... again!",
                        DatePosted = DateTime.Now.AddYears(-1),
                        FeatureImagePath = "/Content/Images/Posts/News/flash.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>"+
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>"+
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "THe Flash film will never get made!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                            },
                            new Comment
                            {
                                Content = "Beep boop io am a robot",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-3),
                            },
                            //a reported comment that has opened a case!
                            new Comment
                            {
                                Content ="THis is a really bad comment and it is filled with swearwords and innapropriate comments!",
                                User = user4,
                                DatePosted = DateTime.Now.AddDays(-42),
                                Reports = new List<Report>
                                {
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1},
                                        Status = Status.Pending
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr2},
                                        Status = Status.Pending
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1,rr2},
                                        ExtraInformation = "How can this even be allowed!",
                                        Status = Status.Pending
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1,rr2,rr4},
                                        ExtraInformation = "This is awful comment and a clear sign of bullying",
                                        Status = Status.Pending

                                    },
                                }
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var news2 = new News
                    {
                        Title = "Major harrasment cases in Hollywood are bleeding out!",
                        Synopsis = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum.",
                        DatePosted = DateTime.Now.AddMonths(-1),
                        FeatureImagePath = "/Content/Images/Posts/News/harvey.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "TIm glad they are getting justice!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-3),
                                EditedAt = DateTime.Now.AddDays(-2),
                            },
                            new Comment
                            {
                                Content = "Beep boop io am a robot",
                                User = user2,
                                DatePosted = DateTime.Now.AddDays(-3),
                            },
                            new Comment
                            {
                                Content = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum.",
                                User = user3,
                                DatePosted = DateTime.Now.AddDays(-3),
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var news3 = new News
                    {
                        Title = "Donec pretium vulputate sapien nec.",
                        Synopsis = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant.",
                        DatePosted = DateTime.Now.AddMonths(-2),
                        FeatureImagePath = "/Content/Images/Posts/News/joker.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "TIm glad they are getting justice!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-32),
                                EditedAt = DateTime.Now.AddDays(-23),
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var news4 = new News
                    {
                        Title = "Steven Spielberg Casts Newcomer Gabriel LaBelle In The Film Inspired By The Director's Life",
                        Synopsis = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant.",
                        DatePosted = DateTime.Now.AddMonths(-3),
                        FeatureImagePath = "/Content/Images/Posts/News/steve.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "TIm glad they are getting justice!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-32),
                                EditedAt = DateTime.Now.AddDays(-23),
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var news5 = new News
                    {
                        Title = "Elizabeth Banks' Red Queen Film Is Now A Streaming Series",
                        Synopsis = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant.",
                        DatePosted = DateTime.Now.AddDays(-1),
                        FeatureImagePath = "/Content/Images/Posts/News/queen.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "TIm glad they are getting justice!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-32),
                                EditedAt = DateTime.Now.AddDays(-23),
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var news6 = new News
                    {
                        Title = "Timothée Chalamet To Play Willy Wonka In Paul King’s Prequel Movie",
                        Synopsis = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant.",
                        DatePosted = DateTime.Now.AddMonths(-5),
                        FeatureImagePath = "/Content/Images/Posts/News/tim.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                       "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                       "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "TIm glad they are getting justice!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-32),
                                EditedAt = DateTime.Now.AddDays(-23),
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var news7 = new News
                    {
                        Title = "Marvel’s Eternals Trailer Brings Chloé Zhao’s Cosmic Vision To The MCU",
                        Synopsis = "Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant.",
                        DatePosted = DateTime.Now.AddMonths(-6),
                        FeatureImagePath = "/Content/Images/Posts/News/eternals.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                       "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                       "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                       "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Comments = new List<Comment>
                        {

                            new Comment
                            {
                                Content = "TIm glad they are getting justice!",
                                User = user1,
                                DatePosted = DateTime.Now.AddDays(-32),
                                EditedAt = DateTime.Now.AddDays(-23),
                            }
                        },
                        Draft = false,
                        Published = true
                    };
                    var newspending = new News
                    {
                        Title = "Yorgos Lanthimos' Poor Things Adds Jerrod Carmichael",
                        Synopsis = "Is The Favourite director Yorgos Lanthimos' Poor Things looking to take over from Knives Out 2 as the film that drops its casting announcements one day at a time? Well, not quite - despite word of Mark Ruffalo yesterday – but Jerrod Carmichael is the latest addition.",
                        DatePosted = DateTime.Now.AddDays(-11),
                        FeatureImagePath = "/Content/Images/Posts/News/jerrod.jpg",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>",
                        Draft = false,
                        Published = false,
                    };

                    var newsSoon = new News
                    {
                        Title = "I am a new post, created 5 mins after you loaded the website!",
                        Synopsis = "I have arrived, and the news is HUGE!",
                        DatePosted = DateTime.Now.AddMinutes(5),
                        FeatureImagePath = "/Content/Images/Posts/News/wow.png",
                        Author = author4,
                        PostContent = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Id aliquet lectus proin nibh nisl condimentum. Lectus nulla at volutpat diam ut. Diam sit amet nisl suscipit adipiscing bibendum est ultricies. Scelerisque purus semper eget duis at tellus at urna condimentum. Accumsan tortor posuere ac ut consequat. In fermentum posuere urna nec tincidunt praesent semper feugiat. Lectus magna fringilla urna porttitor rhoncus. Ultrices neque ornare aenean euismod elementum nisi quis eleifend quam. Et malesuada fames ac turpis egestas integer eget aliquet nibh. Ornare suspendisse sed nisi lacus. Diam sollicitudin tempor id eu nisl nunc mi ipsum faucibus. Cras semper auctor neque vitae tempus. Porttitor lacus luctus accumsan tortor posuere. Eu nisl nunc mi ipsum faucibus vitae aliquet nec ullamcorper. Turpis tincidunt id aliquet risus feugiat in ante.</p>" +
                        "<p>Scelerisque eu ultrices vitae auctor eu augue ut lectus. Tristique senectus et netus et malesuada fames ac turpis egestas. Enim blandit volutpat maecenas volutpat blandit. Nunc sed augue lacus viverra. Justo donec enim diam vulputate ut. Posuere lorem ipsum dolor sit amet. Felis imperdiet proin fermentum leo vel orci. Dictum varius duis at consectetur lorem donec massa sapien. Tempus egestas sed sed risus pretium quam vulputate dignissim. Amet volutpat consequat mauris nunc congue nisi vitae. Enim nec dui nunc mattis enim. Sit amet dictum sit amet justo donec enim. Mauris commodo quis imperdiet massa tincidunt nunc pulvinar sapien et. Ipsum dolor sit amet consectetur adipiscing elit duis tristique. Donec ac odio tempor orci dapibus ultrices. Nam at lectus urna duis convallis convallis tellus id. Vel pretium lectus quam id leo in vitae.</p>" +
                        "<p>Tempor commodo ullamcorper a lacus vestibulum sed. Cursus sit amet dictum sit amet justo donec enim. Consequat ac felis donec et odio. Magnis dis parturient montes nascetur. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Nec nam aliquam sem et tortor consequat id porta. Facilisis gravida neque convallis a cras semper auctor neque vitae. Risus commodo viverra maecenas accumsan. Aliquam purus sit amet luctus venenatis lectus. Lacus suspendisse faucibus interdum posuere lorem ipsum dolor sit. Dictum fusce ut placerat orci nulla. Sed felis eget velit aliquet sagittis id consectetur purus. Urna et pharetra pharetra massa massa ultricies mi quis. Eget nullam non nisi est sit amet facilisis. Cras pulvinar mattis nunc sed blandit libero volutpat sed cras. Faucibus in ornare quam viverra orci sagittis eu volutpat. Malesuada bibendum arcu vitae elementum curabitur.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Donec pretium vulputate sapien nec sagittis aliquam malesuada bibendum. Quam quisque id diam vel quam elementum pulvinar etiam. Sollicitudin aliquam ultrices sagittis orci a scelerisque. Libero nunc consequat interdum varius sit amet mattis vulputate enim. Etiam tempor orci eu lobortis elementum nibh. Nunc vel risus commodo viverra maecenas accumsan lacus. Nullam eget felis eget nunc lobortis. Ultrices dui sapien eget mi proin sed libero. Amet commodo nulla facilisi nullam vehicula ipsum. Mollis aliquam ut porttitor leo a diam sollicitudin tempor. Lacus suspendisse faucibus interdum posuere lorem. At imperdiet dui accumsan sit amet nulla. Dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras tincidunt. Morbi tincidunt ornare massa eget egestas. Tempor orci eu lobortis elementum nibh tellus molestie nunc. Ut tellus elementum sagittis vitae et leo duis. Massa sed elementum tempus egestas. Adipiscing vitae proin sagittis nisl. Cras fermentum odio eu feugiat pretium nibh ipsum consequat nisl.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>" +
                        "<p>Rhoncus aenean vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Purus sit amet volutpat consequat. Lobortis mattis aliquam faucibus purus in massa. Risus in hendrerit gravida rutrum quisque non tellus orci ac. Fringilla phasellus faucibus scelerisque eleifend donec pretium. Interdum velit euismod in pellentesque massa placerat. In aliquam sem fringilla ut morbi tincidunt augue. Ante in nibh mauris cursus. In aliquam sem fringilla ut. Sollicitudin tempor id eu nisl nunc mi ipsum faucibus vitae. Arcu cursus euismod quis viverra nibh cras pulvinar mattis. Aliquet risus feugiat in ante metus dictum at. Tincidunt id aliquet risus feugiat in ante metus dictum at. Tempor orci dapibus ultrices in iaculis nunc sed. Tortor consequat id porta nibh venenatis cras sed felis eget.</p>",
                        Draft = false,
                        Published = true,
                        Editor = editor
                    };

                    context.Posts.Add(newsSoon);
                    context.Posts.Add(news1);
                    context.Posts.Add(news2);
                    context.Posts.Add(news3);
                    context.Posts.Add(news4);
                    context.Posts.Add(news5);
                    context.Posts.Add(news6);
                    context.Posts.Add(news7);
                    context.Posts.Add(newspending);
                    context.SaveChanges();


                    //CASES
                    //case assinged to a mod
                    //add a comment to a post to be part of the case
                    var badComment = new Comment
                    {
                        Content = "this is s**t! Absolute f***kers",
                        User = user3,
                        DatePosted = DateTime.Now.AddDays(-42),
                        Post = review1,
                        Reports = new List<Report>
                                {
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1},
                                        Status = Status.Assigned
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr2},
                                        Status = Status.Assigned
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1,rr2},
                                        ExtraInformation = "How can this even be allowed!",
                                        Status = Status.Assigned
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1,rr2,rr4},
                                        ExtraInformation = "This is awful comment and a clear sign of bullying",
                                        Status = Status.Assigned

                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1,rr2,rr4},
                                        Status = Status.Assigned
                                    },
                                }
                        
                    };

                    var anotherBadComment = new Comment
                    {
                        Content = "wow what a bunch of losers",
                        User = user3,
                        DatePosted = DateTime.Now.AddDays(-32),
                        Post = review2,
                        Reports = new List<Report>
                                {
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr1},
                                        Status = Status.Assigned
                                    },
                                    new Report
                                    {
                                        Reasons = new List<ReportReason> {rr2},
                                        Status = Status.Assigned
                                    }
                                }

                    };


                    context.Comments.Add(badComment);
                    context.Comments.Add(anotherBadComment);

                    var openCase1 = new Case
                    {
                        IsGuilty = false,
                        IsSolved = false,
                        User = user3,
                        Moderator = moderator,
                        ReportedComments = new List<Comment> { badComment, anotherBadComment}
                    };

                    context.Cases.Add(openCase1);

                    context.SaveChanges();

                    foreach (Report r in badComment.Reports)
                    {
                        r.Case = openCase1;
                        context.Entry(r).State = EntityState.Modified;
                    }

                    foreach (Report r in anotherBadComment.Reports)
                    {
                        r.Case = openCase1;
                        context.Entry(r).State = EntityState.Modified;
                    }

                    context.SaveChanges();


                    //case unassinged
                }

            }
        }
    }
}