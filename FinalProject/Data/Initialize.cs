using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Data
{
    public class Initialize
    {
        public static async Task Initial(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Initialize default Role DB
            if (!context.Roles.Any())
            {
                string[] roles = new string[] { "Admin", "Author", "Regular" };

                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

            }

            // Initialize default Users DB
            if (!context.Users.Any())
            {
                //Create Admin User
                var chkUser = await userManager.CreateAsync(new ApplicationUser()
                {
                    Id = "1",
                    UserName = "Admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "istrator",
                    Email = "Admin@gmail.com",
                    BirthDate = new DateTime(2001, 01, 01, 9, 0, 0),
                    Gender = "Female"
                }, "Admin123123!");


                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    ApplicationUser admin = await userManager.FindByEmailAsync("Admin@gmail.com");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

                //Create Author User
                chkUser = await userManager.CreateAsync(new ApplicationUser()
                {
                    Id = "2",
                    UserName = "Henbs555@gmail.com",
                    FirstName = "Hen",
                    LastName = "Benshimol",
                    Email = "Henbs555@gmail.com",
                    BirthDate = new DateTime(1994, 07, 03, 14, 0, 0),
                    Gender = "Female"
                }, "Hen123456!");

                //Add default User to Role Author   
                if (chkUser.Succeeded)
                {
                    ApplicationUser admin = await userManager.FindByEmailAsync("Henbs555@gmail.com");
                    await userManager.AddToRoleAsync(admin, "Author");
                }

                //Create Author User
                chkUser = await userManager.CreateAsync(new ApplicationUser()
                {
                    Id = "3",
                    UserName = "Yarden11bitan@gmail.com",
                    FirstName = "Yarden",
                    LastName = "Bitan",
                    Email = "Yarden11bitan@gmail.com",
                    BirthDate = new DateTime(1996, 07, 21, 20, 0, 0),
                    Gender = "Female"
                }, "Yarden123456!");

                //Add default User to Role Author   
                if (chkUser.Succeeded)
                {
                    ApplicationUser admin = await userManager.FindByEmailAsync("Yarden11bitan@gmail.com");
                    await userManager.AddToRoleAsync(admin, "Author");
                }

            }

            // Initialize Teams DB
            if (!context.Teams.Any())
            {
                var teams = new Team[]
                {
                    new Team{TeamID=1, Name="Germany", Description="---Germany - 1,558 Points", Zoom=3, Lattitude=50.744407, Longtidute=10.106440},
                    new Team{TeamID=2, Name="Brazil", Description="---Brazil - 1,431 Points", Zoom=3, Lattitude=-10.093902, Longtidute=-56.183146},
                    new Team{TeamID=3, Name="Belgium", Description="---Belgium - 1,298 Points", Zoom=3, Lattitude=50.656166, Longtidute=4.603757},
                    new Team{TeamID=4, Name="Portugal", Description="---Portugal - 1,274 Points", Zoom=3, Lattitude=50.656166, Longtidute=4.603757},
                    new Team{TeamID=5, Name="Argentina", Description="---Argentina - 1,241 Points", Zoom=3, Lattitude=-34.493360, Longtidute=-65.980877},
                    new Team{TeamID=6, Name="Switzerland", Description="---Switzerland - 1,199 Points", Zoom=3, Lattitude=46.700945, Longtidute=7.917856},
                    new Team{TeamID=7, Name="France", Description="---France - 1,198 Points", Zoom=3, Lattitude=46.379985, Longtidute=2.473094},
                    new Team{TeamID=8, Name="Poland", Description="---Poland - 1,183 Points", Zoom=3, Lattitude=52.608496, Longtidute=18.542962},
                    new Team{TeamID=9, Name="Chile", Description="---Chile - 1,135 Points", Zoom=3, Lattitude=-27.407033, Longtidute=-70.271237},
                    new Team{TeamID=10, Name="Spain", Description="---Spain - 1,126 Points", Zoom=3, Lattitude=39.994922, Longtidute=-3.649235}
                };

                foreach (Team t in teams)
                {
                    context.Teams.AddRange(t);
                }

                context.SaveChanges();
            }

            // Initialize Articels DB
            if (!context.Articles.Any())
            {
                /*  var articels = new Article[]
                   {
                       new Article {
                           ID = 1,
                           Title = "Mick Fanning surfs the Eisbach River wave in Munich",
                           Text =  "Mick Fanning: he loved the Eisbach River wave | Photo: Wilson/Red Bull Three-time world champion Mick Fanning went surfing at the Eisbach River, in Munich, Germany.",
                           Image = "fanningeisbach.jpg",
                           Video = "Mick-Fanning-surfs-in-Munich.mp4"
                       },
                       new Article {
                           ID = 2,
                           Title = "Cristiano Ronaldo's ambition means he could play at Qatar 2022",
                           Text = "SOCHI, Russia -- The 'will he, won't he?' debate about whether Cristiano Ronaldo will attempt to play in a fifth World Cup in four years'",
                           Image = "ronaldo 2.jpg"
                         },
                       new Article {
                           ID = 3,
                           Title = "WAS THIS THE BIGGEST NY EVER?",
                           Text = "More than a decade ago, Will Skudin and a crew of East Coast hellmen attempted the paddle to Montauk's outer sandbars, as a macking swell rifled into shore.",
                           Image = "3.jpg",
                           Video = "3.mp4"
                         },
                       new Article {
                           ID = 4,
                           Title = "EDDIE WON'T GO BUT MAVERICKS IS ON",
                           Text = "The Quiksilver in Memory of Eddie Aikau big wave event will not run in the 2017/2018 season, it has been confirmed.",
                           Image = "4.jpg",
                           Video = "4.mp4"
                           },
                       new Article {
                           ID = 5,
                           Title = "HOW TO SURVIVE THE DREADED SKUNK ATTACK",
                           Text = "If you travel for surf, it’s going to happen eventually—the dreaded skunk. Perhaps the wind goes sour or the weather doesn’t cooperate.",
                           Image = "5.jpg"
                         },
                       new Article {
                           ID = 6,
                           Title = "TEAM USA WINS 2017 VISSLA ISA WORLD JUNIOR SURFING CHAMPIONSHIP",
                           Text = "Team USA has been crowned the Team World Champion at the 2017 Vissla ISA World Junior Surfing Championship in Hyuga, Japan.",
                           Image = "6.jpg",
                           Video = "6.mp4"
                         },
                       new Article {
                           ID = 7,
                           Title = "WAITING PERIOD OPENS FOR NELSCOTT REEF BIG WAVE PRO-AM",
                           Text = "Although we haven’t seen any huge swells yet, the north Pacific is starting to wake up, with various systems forming north of Hawaii and the Pacific Northwest.",
                           Image = "7.jpg"
                         },
                       new Article {
                           ID = 8,
                           Title = "The Wedge shows its claws",
                           Text = "We're already deep into fall, but Southern Californians are calling it the swell of the summer.",
                           Image = "8.jpg",
                           Video = "8.mp4"
                         },
                       new Article {
                           ID = 9,
                           Title = "How to maintain your foil",
                           Text = "Learn simple preventive measure that will make your foil last longer.",
                           Image = "9.jpg",
                           Video = "9.mp4"
                         },
                       new Article {
                           ID = 10,
                           Title = "Mavericks included in the 2017/2018 Big Wave Tour",
                           Text = "It's official. The Mavericks big wave surfing competition is officially part of the World Surf League (WSL).",
                           Image = "10.jpg",
                           Video = "10.mp4"
                         },
                       new Article {
                           ID = 11,
                           Title = "Soul Surfer",
                           Text = "It came, literally, out of the blue.  ",
                           Image = "soul-surfer.jpg",
                           Video = "Soul-Surfer.mp4"
                         }
                   };

                   foreach (Article a in articels)
                   {
                       context.Articles.AddRange(a);
                   }

                   context.SaveChanges();*/
            }
        }
    }
}
