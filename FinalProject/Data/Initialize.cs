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
                    new Team{Name="Germany", Description="---Germany - 1,558 Points", Zoom=3, Lattitude=50.744407, Longtidute=10.106440},
                    new Team{Name="Brazil", Description="---Brazil - 1,431 Points", Zoom=3, Lattitude=-10.093902, Longtidute=-56.183146},
                    new Team{Name="Belgium", Description="---Belgium - 1,298 Points", Zoom=3, Lattitude=50.656166, Longtidute=4.603757},
                    new Team{Name="Portugal", Description="---Portugal - 1,274 Points", Zoom=3, Lattitude=50.656166, Longtidute=4.603757},
                    new Team{Name="Argentina", Description="---Argentina - 1,241 Points", Zoom=3, Lattitude=-34.493360, Longtidute=-65.980877},
                    new Team{Name="Switzerland", Description="---Switzerland - 1,199 Points", Zoom=3, Lattitude=46.700945, Longtidute=7.917856},
                    new Team{Name="France", Description="---France - 1,198 Points", Zoom=3, Lattitude=46.379985, Longtidute=2.473094},
                    new Team{Name="Poland", Description="---Poland - 1,183 Points", Zoom=3, Lattitude=52.608496, Longtidute=18.542962},
                    new Team{Name="Chile", Description="---Chile - 1,135 Points", Zoom=3, Lattitude=-27.407033, Longtidute=-70.271237},
                    new Team{Name="Spain", Description="---Spain - 1,126 Points", Zoom=3, Lattitude=39.994922, Longtidute=-3.649235}
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
                  var articels = new Article[]
                   {
                       new Article {
                           Title = "Should Lionel Messi''s failure to win a World Cup affect his GOAT claim?",
                           Text =  "MOSCOW -- Is this the last we''ll see of Lionel Messi on football''s biggest stage, the World Cup? Argentina are out, and with them go a seemingly demoralized, I''d-rather-be-anywhere-than-Russia Lionel Messi. True, he did what he could in the round of 16 with two assists, including a beauty that lead to Kun Aguero''''s goal. But it wasn''''t enough. Not even close.\nAnd so the countdown begins. Lionel Messi will be 35 years and 208 days old on Dec. 18, 2022, the day he theoretically gets his next crack at winning the World Cup. Only three players older than that have started a World Cup final and won: Nilton Santos in 1962, Dino Zoff in 1982 and Miroslav Klose in 2014.\n\nThat means it has been done before. Sure, it''''s not an easy thing to do. But this is Messi we''''re talking about. There''''s difficult and then there''''s Messi with the five Ballon d''''Ors, the four Champions Leagues and the 552 goals for Barcelona.\n\nThink that''''s easy?\n\nAnd yet as great as those achievements were, they were different. Zoff, of course, was a goalkeeper: different rules apply to them. Nilton Santos was the veteran on an uber-dominant Brazil that included the likes of Didi, Zito, Garrincha, Djalma Santos and, until his injury, Pele. Klose started three games during Germany''''s run, scoring one goal. They had far better supporting casts than what Messi is likely to have in 2022.\n\nMore importantly, perhaps, while they were valued team members who made important contributions, these weren''''t key men. In other words, these weren''''t heroes who carried their team to glory\n\nBut that''''s exactly what Messi has been, what Messi is and what Messi likely will be, although perhaps not on December 18, 2022. Biology isn''''t an opinion or a state of mind. We age: our bodies degrade and with them, our ability to make them do what we want.\n\nSome can slow the process, whether because they are freaks of nature or whether because they are maniacal in the care they take of their bodies -- a certain Cristiano Ronaldo, 33, comes to mind -- or whether because they are just fortunate. But nobody can halt it. Not even Messi.\n\nWith this elimination comes the knowledge that he won''''t win the World Cup as a protagonist. He won''''t follow in the path of Pele and Diego Maradona. (And because the debate is inevitable to some, no, Cristiano Ronaldo may not do it either. But he''s in a different category merely by the fact that he''s Portuguese and Portugal is not a superpower on the level of Brazil or Argentina. Ronaldo also led Portugal to Euro glory.).\n\nDoes Messi''s failure at the 2018 World Cup matter to the GOAT debate?\n\nRealistically, it shouldn''t. It''s a foolish criteria, laid out by those who don''t seem to understand chance and probability and what a single individual can and cannot control. Zinedine Zidane won a World Cup in 1998 because his teammates beat Paraguay in extra-time without him and because they beat Italy on penalty kicks. He lost a final in 2006 because, again, without him, they lost to Italy on penalty kicks.\n\nMessi can replay the 2014 final against Germany in his head and no doubt come up with dozens of moments where things could have gone differently and broken his way, many of them outside his control: the most obvious being Gonzalo Higuain''s missed chance.\n\nYet football doesn''t work that way. It is a team sport. You can judge individuals on individual achievements, but doing so on collective achievements is fraught with peril and imprecision.\n\nMaybe, on his way home from Russia, Messi is thinking of his four World Cups and what could have been different. In 2006, he was a teenager, with a mere 11 league starts to his name. He made three appearances, two as a substitute and scored a goal. Roberto Abbondanzieri''s injury in goal and Jose Pekerman''s decision to send on Julio Cruz instead of Messi meant he played no part in the quarterfinals against Germany. The spot kick defeat meant he went home.\n\nSouth Africa 2010 was marked by Maradona''s follies as Argentina coach. Messi played every minute of every game, failing to score (but assisting plenty in a deeper playmaking role) and crashed out again in the quarterfinals, again to Germany.\n\nThen there was Brazil 2014. By now, he had the captain''s armband and despite arriving at the World Cup carrying an injury, loaded the Albiceleste on his back through the group stage and into the final, where they succumbed to -- who else? -- Germany. Of little consolation, Messi was awarded the Golden Ball for best player in the tournament.\n\nNow this World Cup. Argentina manager Jorge Sampaoli has replicated the chaos of the 2010 edition, with the minor difference that he isn''t an icon like Maradona, and therefore every mistake has been amplified and magnified. Maybe Sampaoli will be the scapegoat -- like Maradona was in 2010, albeit after the fact -- or maybe we''ll simply accept that this isn''t a great generation of Argentine players.\n\nYet deep down Messi must know -- in the way that footballers who are honest with themselves know -- that his missed penalty against Iceland and his nonexistent performance against Croatia are the reason Nigeria became a must-win game. Sampaoli has his responsibilities, sure, but so does the guy who proves to be human when for the longest time he performed as if superhuman. Argentina managed to beat Nigeria dramatically, 2-1, with a goal from Messi. There was hope, until France -- and Kylian Mbappe, the World Cup''s new sensation -- had different ideas.\n\nThe inquest will begin, with arguments on both sides. Reaching four major finals -- losing two of them on penalties and one in extra-time -- won''t convince those who say he ought to have done more for his country. In his heart -- because athletes on his level are often, at least privately, their own harshest critics - Messi may second-guess himself, particularly in this tournament.\n\nWhen he was younger, his loyalty -- even his patriotism -- was called into question by some in Argentina. That''s what happens when you leave at 13 and achieve more with your club than with your country.\n\nQuestioning Messi''s love for his nation is, of course, silly, particularly when you consider the times he played hurt and the times he took a public stand (witness his brief retirement in protest against the chaos at the Argentina FA after the 2016 Copa America). Yet that, too, is part of the narrative.\n\nIf there is one easy conclusion to make it''s this: winning when you''re part of a perennially well-resourced and organized framework like he enjoys at Barcelona is a whole heck of a lot easier. It''s not just about getting your tactical instructions from Pep Guardiola rather than Maradona or the fact that if you need a central midfielder you can buy an Ivan Rakitic rather than being forced to make do with a Lucas Biglia.\n\nIt''s the fact that, maybe more than most, chaos and instability don''t suit him.\n\nThink back to what was, arguably, Messi''s worst performance before the Croatia match: the Champions League second leg against Roma in April, when Barcelona contrived to squander a 4-1 first leg lead and lose 3-0. There, too, you can shift blame to the manager for his tactics, praise the opposition for raising their game, celebrate the magical unpredictability of the sport. But there is no denying a woeful, absent, listless performance from Messi, either.\n\nIt happens, many said. He''s entitled a day off. The problem is that as you age, nights like Rome and Nizhny Novgorod risk becoming more frequent.\n\nThat''s time for you. Rocky Balboa was right: Time is undefeated.\n\nMessi can''t make time stop for him. But maybe, just maybe, he can make it slow down.\n\nMaybe he can take a mulligan and have another crack in 2022. For his own benefit, for those who love to watch him play and to shut up all those who judged a man by medals won in July, rather than by actions performed over a lifetime.",
                           Image = "art1.jpg",
                           PublishDate = new DateTime(2018, 06, 20, 9, 0, 0),
                           Author = "Hen Benshimol",
                           AuthorID = "2"
                       },
                       new Article {
                           Title = "Cristiano Ronaldo''s ambition means he could play at Qatar 2022",
                           Text = "'SOCHI, Russia -- The ''will he, won''t he?'' debate about whether Cristiano Ronaldo will attempt to play in a fifth World Cup in four years'' time can start in earnest following Portugal''s elimination from Russia 2018 at the round-of-16 stage by Uruguay.\nIt has already begun for Lionel Messi, who was also given his boarding pass for a flight home on Saturday when Argentina fell to a 4-3 defeat vs. France in Kazan -- but the two superstars have wholly different issues to consider before making a decision regarding their international future.\n\nMessi came to life in Argentina''s do-or-die group game against Nigeria, scoring a stunning goal and inspiring a crucial 2-1 victory, but has often looked crushed by the weight of expectancy that comes with leading a football nation as demanding and historically successful as Argentina.\n\nRonaldo, however, appears energised whenever he wears the Portuguese badge on his chest. He is the global image of his nation and relishes the pressure that comes with being the man expected to deliver. And the big question is whether he is ready -- or even contemplating -- to give all of that up.\n\nRonaldo will be three months short of his 38th birthday when Qatar 2022 kicks off, but, while it remains to be seen if he will be there, Portugal coach Fernando Santos is confident his captain will continue beyond Russia.\n\n''Cristiano still has a lot to give to football,'' said Santos. ''There is a tournament [UEFA Nations League] in September, and we hope Cristiano will be with us to help the players grow.''\n\nWhereas Ronaldo is loved and cherished by Portugal, Messi plays for a poor Argentina team that is unlikely to be good enough to win a World Cup until 2026 at the earliest, and the Barcelona forward will be long gone by then. \n\nArgentina have not won Copa America for 25 years, and so every time Messi pulls on his country''s shirt, he knows he is representing a failing generation that has proven unable to match the feats of the 1978 World Cup winners, nor the Diego Maradona - inspired world champions of eight years later or the continental champions of 1991 and 1993.\n\nRonaldo does not have to shoulder the same burden.Portugal, ironically after their talisman limped off early due to injury in the final against France, achieved their dreams by winning Euro 2016, so this small European nation has already savoured a success it perhaps never truly imagined.\n\nForty million Argentinians will demand to know why Messi & Co.are returning home early after failing to win it for a third time, whereas nobody really envisaged Portugal as World Cup winners in Russia.\n\nIt means Ronaldo can go home knowing his status as a national hero is secure, so what happens next is all about his own ambition and insatiable appetite for creating records and raising his own bar.\n\nNot that this latest exit, which came courtesy of two Edinson Cavani wonder goals for Uruguay, will have not hurt.Ronaldo''s best chance to win the game''s biggest prize probably came in 2006, when a great team including Luis Figo, Deco and Ricardo Carvalho suffered a 1 - 0 semifinal defeat against France.\n\nAnd so, as Messi and Ronaldo consider what to do next, there is a difference: Messi must ask if he wants to continue to expose himself to the draining pressure of playing for Argentina, while Ronaldo''s dilemma is whether a four - year wait for Qatar is a price worth paying for the adulation he gets whenever he plays for Portugal.",
                           Image = "art2.jpg",
                           PublishDate = new DateTime(2018, 06, 15, 9, 0, 0),
                           Author = "Hen Benshimol",
                           AuthorID = "2"
                         },
                       new Article {
                           Title = "Kylian Mbappe announces himself on World Cup stage with Ronaldo-like performance vs. Argentina",
                           Text = "KAZAN, Russia -- Did that remind you of anyone? As Kylian Mbappe set off, capitalising on Ever Banega''s bad touch and scorching away from his pursuers in such a manner that it was clear nobody could hold any hope of stopping him legally, a figure from the past loomed into mind. Has anyone caused defences this much terror when in full flight since the best centre-forward of the past 20 years, the magnificent Brazilian Ronaldo?\n\nHyperbole can be tiresome, but the problem was that this occasion lent itself to bold statements. \"People feel like comparing players,\" France manager Didier Deschamps said in his postmatch news conference before opting to satisfy the first journalist who brought up the \"R\" word. \"Ronaldo was a forward player who was very, very quick. I think Kylian is even quicker. But this is somebody who was a world champion compared to a young player who has lots of abilities. He''ll make a lot of progress, but I''m already very happy with the way he played.\"\n\nHe was right to be. Deschamps was probably unaware that Ronaldo was watching from the stands, sitting next to Diego Maradona and probably watching his companion boil over with a mixture of apoplexy and admiration. Argentina could not handle Mbappe, barely landing a legal tackle on him in the first half and then failing to lay a glove by fair means or foul as he outstripped them to score twice after the break.\n\nAt times, when Mbappe was coming through at Monaco in the 2017-18 season, it seemed a degree of caution should be reserved in the expectations set for a player who turned 18 halfway through the season. The £166 million fee Paris Saint-Germain subsequently agreed for him appeared to be asking for trouble. But a performance like this on the highest stage suggests something tantalising: that all the hype could not be more real.\n\n\"He adores football,\" said Deschamps, who was clearly delighted by the way in which his tactical setup had allowed Mbappe to be so thrillingly effective. \"He knows everything about clubs and players, and I''ve always said he''s very good. He has a lot of room to make progress but, in such an important match, he''s shown all his talent. And even if he was supposed to defend, he still made attacks-- and very good ones.\"\nThat hinted at perhaps the most exciting element of Mbappe''s potential. Those who have worked with him speak glowingly of his seriousness, his intelligence, his contentment with working to a plan. He did just that, as his manager said, doing the dirty work here and picking his moments to cut loose. Perhaps that was easier in the knowledge that, whenever he did, he would have a creaking Argentina side on toast. On the left, down the right or through the middle -- Jorge Sampaoli''s players could not cope when faced with him and it will be hard to find a more complete individual performance for the rest of this tournament.\n\nFrance may need one close to it, though. They will not face as obliging a defence as Argentina''s in the quarterfinals and certainly have enough mistakes in them -- as shown through their sluggishness in closing down Angel Di Maria for his goal here -- to ensure that their front men are required to be on song. Those who like omens will note that Mbappe was born in December 1998, the same year France last lifted the World Cup. If they want to take things further they might note that it was 20 years to the day since Michael Owen, with a solo run of his own that bore resemblance to Mbappe''s pitch-length sprint, made his own indelible mark on the world stage. The signs are good, but it will have been particularly encouraging for Deschamps that Antoine Griezmann was on song here, too, and that Paul Pogba -- who released Mbappe with one breathtaking 60-yard pass in the first half -- appeared well in tune with the needs of his quicksilver teammates.\n\n\"The Argentinian team is much better in attacks than in its defending,\" Deschamps admitted afterward, but he and his players deserve considerable credit for the manner in which they exposed that fact so brutally. He said such a young group, 14 of whom had not been part of a World Cup before, needed \"a little patience, a little indulgence\" to succeed. \"Now, everyone has to be top - notch[if we are to succeed],\" he continued. Expectations will naturally go through the roof from hereon, though, and it is difficult to see a side that can match the tempo France set here if they can keep it going for another fortnight.\n\nMbappe held a short news conference of his own before Deschamps'' arrival; he was obliged to do so after being named man of the match. He was instantly informed that, with Pele, he is one of only two teenagers to have scored twice in a World Cup knockout game. \"Let''s put things into context, Pele is another category,\" he said. Perhaps it was best that he was not around several minutes later to dampen the Ronaldo buzz. Pick your favourite legendary Brazil forward: Mbappe is progressing at such a rate that comparisons with either may one day be borne out after all.",
                           Image = "art3.jpg",
                           PublishDate = new DateTime(2018, 06, 28, 9, 0, 0),
                           Author = "Hen Benshimol",
                           AuthorID = "2"
                         },
                       new Article {
                           Title = "Uruguay''s Edinson Cavani strikes twice to send Cristiano Ronaldo and Portugal out",
                           Text = "Two goals from Edinson Cavani earned Uruguay a place in the World Cup quarterfinals as they ended the dreams of Cristiano Ronaldo and Portugal with a 2-1 win in Sochi.\n\nCavani headed home before the break and, soon after Pepe had headed a second-half equaliser, scored the winner with a superb curling shot.\n\nPortugal had been first to threaten, Joao Mario getting down the left and crossing to the far post where Bernardo Silva could not steer his header on target.\n\nWith six minutes gone, Bernardo Silva made progress down the other flank and laid the ball off to Ronaldo, who blasted a first-time effort straight at keeper Fernando Muslera.\n\nBut a minute later, Uruguay led when Cavani and Luis Suarez combined in style, Cavani''s pass finding Suarez and his cross converted by the striker at the far post, the ball seeming to go in off his face.\n\nAt the other end, Jose Fonte steered a downward header across goal and behind, although he appeared to be fouling defender Matias Vecino, and at the other end Suarez saw a cross deflected over after another sharp run had created danger.\n\nBack came Portugal but, after a good spell of pressure, Joao Mario''s cross was too deep for Ronaldo and Muslera claimed.\n\nBernardo Silva and Goncalo Guedes then combined well but the latter''s cross was headed clear before it could reach Ronaldo.\n\nAs play switched to the other end, Fonte brought down Suarez for a dangerous 25-yard free kick and the forward''s low shot was well saved by Rui Patricio.\n\nRicardo Carvalho''s free kick eluded both Ronaldo and Fonte, with the half-hour approaching and Portugal yet to create a clear-cut chance.\n\nRonaldo then had an opening when Rodrigo Bentancur conceded a free kick for a foul on Guedes 25 yards out, only to blast his effort into the wall.\n\nMartin Caceres and Nahitan Nandez combined to create a chance but the move ended when Cavani was unable to control an awkwardly-bouncing ball, and with five minutes remaining until the break Joao Mario made a promising run down the left only to slip.\n\nIn the final moments of the half, with Suarez down following a challenge from Raphael Guerreiro, Portugal won a corner that came to nothing before a half-chance was steered wide by Cavani at the far post.\n\nDiego Godin cleared as Joao Mario sent in the first cross of the second half, and Ricardo put in another that was dealt with by Godin before it could find the waiting Ronaldo.\n\nGuerreiro fired over from the edge of the box after a corner had fallen to him, and then Ronaldo laid the ball back to Adrien Silva, whose shot was deflected behind for a 55th-minute corner.\n\nFrom it, Portugal were level as Pepe stormed in to head home from Guerreiro''s delivery and grab the goal that they had increasingly threatened since the break.\n\nBut parity did not last long, Cavani steering a brilliant curling finish into the corner after 62 minutes following Bentancur''s beautifully-weighted pass into his path.\n\nCristian Rodriguez came on for Bentancur as Uruguay made the first change, Portugal taking off Adrien Silva and bringing on Ricardo Quaresma soon afterwards.\n\n\nWith 20 minutes remaining, Bernardo Silva hooked over after Muslera had failed to hold a ball in the area under pressure, and then a Ronaldo strike from outside the box came back off a defender.\n\nGuedes made way for Andre Silva and goal hero Cavani, limping after a knock, was replaced by Cristhian Stuani befoe Guerreiro sent another effort over as the game entered its final quarter of an hour.\n\nBernardo Silva drove in a low cross that deflected over as Portugal began to run out of time, and then Nandez was replaced by Carlos Sanchez for Uruguay.\n\nQuaresma bent a cross towards Ronaldo, but just over him, and Bernardo Silva''s low ball in was cleared before Manuel Fernandes came on for Portugal in place of Joao Mario.\n\nRonaldo sliced another attempt wide and was booked for protesting after the referee declined to give a foul on Quaresma -- but neither he nor Portugal could find a way through in four minutes of stoppage time as Uruguay held on to their lead to set up a quarterfinal against France on Friday.",
                           Image = "art4.jpg",
                           PublishDate = new DateTime(2018, 07, 03, 9, 0, 0),
                           Author = "Hen Benshimol",
                           AuthorID = "2"
                         },
                       new Article {
                           Title = "Horrible host history not for Hierro as Spain face Russia",
                           Text = "MOSCOW, June 30 (Reuters) - Fernando Hierro doesn''t want to know about Spain''s horrible history of playing the hosts of major tournaments as his side prepare to face Russia in the World Cup last 16 in Moscow on Sunday.\n\n\"Records are there to be broken,\" the coach told a reporter on Saturday who told him that Spain have never beaten a host at the World Cup or European Championship.\n\n\"We have everything very clearly in mind,\" Hierro said, insisting he was ready for anything Russia could throw at him.\n\n\"Why are we looking in the rear view mirror? Why are we looking at the past? It''s all about tomorrow, 5 p.m. Everything else is completely irrelevant.\"\n\nMost painfully in recent memory, Spain lost to South Korea on penalties in the 2002 World Cup. They were also beaten by Portugal in Euro 2004 and by England -- on penalties -- at Euro 96.\n\nHierro waved away questions about his side''s stuttering performances after he was promoted at the last minute to replace Julen Lopetegui, fired on the eve of the tournament after accepting a job at Real Madrid. Spain won their group after beating Iran 1-0 and drawing with Morocco and Portugal.\n\n\"Football is made of mistakes and the team that makes fewer mistakes probably wins,\" he said. \"We need to avoid mistakes.\"\n\nMidfielder David Silva also defended Spain''s record in Russia.\n\n\"All teams are having a hard time,\" he said. \"Sometimes teams park the bus in front of goal. It can be hard to shine because of that kind of behaviour.\"\n\nAgainst Russia, with both sides needing to score, Silva believes that Spain will get chances.\n\n\"If we play very fast up front we''ll have a lot of options and we''ll get space and we''ll hurt them,\" he said.\n\nHierro dismissed fears of playing in front of a vocal home crowd.\n\n\"My lads are used too playing in big stadiums, where there''s a lot of pressure, where you''re away from home,\" he said. \"Everything that will be achieved will be on the pitch.\" (Reporting by Alastair Macdonald; editing by Ed Osmond)",
                           Image = "art5.jpg",
                           PublishDate = new DateTime(2018, 06, 21, 9, 0, 0),
                           Author = "Yarden Bitan",
                           AuthorID = "3"
                         },
                       new Article {
                           Title = "Confident Belgium wary of threat from Japan",
                           Text = "'MOSCOW, June 30 (Reuters) - Belgium go into Monday''s World Cup last 16 match against Japan full of confidence but wary that relying on individual talent against similarly well-organised but less fancied teams has cost them dear in the past.\n\nThe two teams come into the knockout phase off the back of very different performances -- Japan embarrassingly strolled through a 1-0 defeat by Poland to advance by having fewer yellow cards than Senegal while Belgium threw calculations to the winds and saw their second-string beat England''s reserve team 1-0, risking a possible quarter-final against Brazil.\n\nUnbeaten in 22 games, Roberto Martinez''s side can take comfort from a 1-0 victory over Japan in a friendly in Belgium last November in which their goal was scored by Romelu Lukaku, who has netted four times in two games in Russia so far and who is available to face Japan after an ankle injury.\n\n\"The opportunity of facing Japan with the mentality that the group has is a very positive moment for us,\" Martinez said ahead of Monday''s match in Rostov-on-Don.\n\nNoting Japan had dramatically changed their coach since last year''s friendly -- Akira Nishino was brought in just two months ago -- he praised the side which beat group winners Colombia in their opening match. \"They''re going to be a competitive team,\" he said. \"They know how to hurt you in a counter - attack.\"\n\nNishino has promised a better performance than the antics in the later stages against Poland. \"(Fans) were short-changed 10 minutes and they probably got a bit less mileage than usual out of the first 80,\" he said. \"So I want to pay people back.\"\n\nAfter many changes against Poland, the Samurai Blue, who have twice reached this stage but have never gone further, should revert to a lineup including captain Makoto Hasebe, Borussia Dortmund''s Shinji Kagawa and Yuya Osako up front.\n\nIt is the first big competitive test of Martinez''s two years of trying to turn a \"golden generation\" of individual talents with big egos at club level into a national side which can finally live up to its top-five FIFA world ranking.\n\n\nWith nine goals and nine points from their three group games, the Belgians are going into the final stages buzzing -- and refusing to regret a win over England that leaves them in what looks a much the tougher half of the draw, which includes Brazil, France and Belgium''s World Cup nemesis Argentina.\n\n\"There''s a real belief,\" the Spaniard said. \"There is no margin of error... But we''re going to do what we believe in.\"\n\nDries Mertens, who is likely to be back alongside Eden Hazard in support of Lukaku after Martinez fielded a virtual reserve team against England, said the players were well aware that they had come up short in the past four years.\n\nThe Red Devils scraped through at this stage of the 2014 World Cup against the United States, winning 2-1 after extra time, midfielder Dries Mertens recalled, then lost the quarter-final 1-0 to Argentina.\n\n\"And remember the Wales game,\" he said of their 3-1 Euro 2016 quarter-final loss. \"Everyone thought we''d go through -- we went out. We have that on our minds and it gives us strength.\" (Editing by Ken Ferris)",
                           Image = "art6.jpg",
                           PublishDate = new DateTime(2018, 06, 25, 9, 0, 0),
                           Author = "Yarden Bitan",
                           AuthorID = "3"
                         },
                       new Article {
                           Title = "France in dreamland after unforgettable World Cup win over Argentina",
                           Text = "In a corner of the jubilant France dressing room in the Kazan Arena, Didier Deschamps grabs Benjamin Pavard, hugs him and tells him: \"You remind me of Lilian Thuram in 1998!\"\n\nThe young defender, 22 years old, not even 10 caps yet, is moved to tears.\n\nTwenty years ago, Thuram, the centre-half turned right-back who had never scored for France before, netted twice in the World Cup semifinal against Croatia. Pavard, Stuttgart''s centre-half, is also a makeshift right-back for Les Bleus and his stunning goal, maybe the best of the tournament so far, was as unexpected and as unprecedented as Thuram''s brace all those years ago.\n\nPavard is in dreamland like the rest of the squad. The dressing room looks like a giant nightclub. The music is loud, everybody is dancing, singing, chatting. The atmosphere is amazing. Before the celebrations and the craziness started, Deschamps, as he always does, spoke first.\n\n\"I am proud of you. The most important is the qualification but the way you did it is remarkable,\" he told his players. \"I liked your team spirit. You never gave up, you kept fighting.\"\n\nHe took the time to shake each of their hands and have a personal word with every one of them. Since he took over in the summer 2012, this is -- along with the win against Germany in the Euro 2016 semifinal -- one of his best moments.\n\nDeschamps is beaming. In his post match news conference, he has a little dig at some of the French journalists for criticising him before the game. Kylian Mbappe doesn''t like the critics either. He didn''t like being criticised after the 2-1 opening win against Australia. It feels almost like another life. Mbappe is the star of the show now.\n\nHis performance against Leo Messi and Argentina was breathtaking, splendid. He cannot believe his ears when he is told about Pele''s tweet about him, congratulating him for being the first teenager since the king himself back in 1958 to score twice in a World Cup game.\n\nIn the dressing room, Paul Pogba comes to congratulate him. Everybody follows. Ousmane Dembele, as usual, is sat next to him. On the other side, it is even wilder. You have Presnel Kimpembe, Antoine Griezmann, Pogba and Benjamin Mendy, all sat next to each other, celebrating. Pogba is on fire!\n\nOnce at the airport, the private plane which takes Les Bleus back to Moscow, is transformed into a giant karaoke. The tune of the night is Siguelo bailando by Ozuna, the Puerto Rican reggaeton and latin trap singer. The French shout the words, they dance. Kimpembe is carrying the big portable speaker. Naza, the French rapper and his song MMM which has a football theme to it, is another one that the whole team scream on!",
                           Image = "art7.jpg",
                           PublishDate = new DateTime(2018, 07, 01, 9, 0, 0),
                           Author = "Yarden Bitan",
                           AuthorID = "3"
                         },
                       new Article {
                           Title = "World Cup Group C: France and Denmark advance as Peru beat Australia",
                           Text = "Denmark 0-0 France\n\n\nDenmark and France played out one of the less memorable games of Russia 2018 so far but a 0-0 draw will have pleased both sides'''' coaches and fans.\n\nThe point was enough to give France top spot in Group C and Denmark came into the game knowing a point would guarantee them a last-16 spot.\n\nFrance are now 15/2 with Sky Bet to win the tournament outright, while the online bookmaker has priced Denmark at 9/1 to reach the semi-finals.\n\nAs it happened, Australia''''s defeat by Peru meant they could have lost this game and still advanced but they now march on and are 18 games unbeaten.\n\nYou can often tell the quality of a match at a packed stadium by when the first Mexican wave starts: 21 minutes was the answer here and it was probably overdue.\n\nBut then this entire World Cup has been overdue a stinker and this game had all the ingredients.\n\nAustralia 0-2 Peru\n\n\nAustralia''''s World Cup last-16 hopes were ended by a 2-0 defeat to Peru, who finished their tournament with a historic win.\n\nThe South Americans, who were yet to score in the tournament, went ahead from their first attack when Watford loanee Andre Carrillo''''s textbook volley beat Mathew Ryan from 15 yards after 18 minutes.\n\nAustralia''s relentless pressure failed to forge any major openings before Peru doubled their lead, when a wild deflection found Paolo Guerrero in the Australia box, and another from his shot wrong-footed goalkeeper Ryan (50).\n\nAustralia forced a couple of half-chances from corners, with Tim Cahill seeing a volley blocked, but the Socceroos'' tournament ended with a whimper, while Peru were left wildly celebrating their first World Cup win since 1978.",
                           Image = "art8.jpg",
                           PublishDate = new DateTime(2018, 07, 02, 9, 0, 0),
                           Author = "Yarden Bitan",
                           AuthorID = "3"
                         },
                       new Article {
                           Title = "Poland 0-3 Colombia: Group H hope for South American nation",
                           Text = "Learn simple preventive measure that will make your foil last longer.",
                           Image = "9.jpg",
                           PublishDate = new DateTime(2018, 07, 05, 9, 0, 0),
                           Author = "Yarden Bitan",
                           AuthorID = "3"
                         }
                   };

                   foreach (Article a in articels)
                   {
                       context.Articles.AddRange(a);
                   }

                   context.SaveChanges();
            }
        }
    }
}
