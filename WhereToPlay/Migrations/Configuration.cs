namespace WhereToPlay.Migrations
{
    using Models.DB;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WhereToPlay.Models.WhereToPlayDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WhereToPlay.Models.WhereToPlayDb context)
        {
            context.Sports.AddOrUpdate(
              p => p.SportName,
              new Sport { SportName = "Fotbal" },
              new Sport { SportName = "Tenis" },
              new Sport { SportName = "Baschet" }
            );

            context.UserGroups.AddOrUpdate(
              p => p.UserGroupName,
              new UserGroup { UserGroupName = "Administrator" },
              new UserGroup { UserGroupName = "Proprietar" },
              new UserGroup { UserGroupName = "Jucator" }
            );

            context.ReservationTimes.AddOrUpdate(
              p => p.Hours,
              new ReservationTime { Hours = "10-12" },
              new ReservationTime { Hours = "12-14" },
              new ReservationTime { Hours = "14-16" },
              new ReservationTime { Hours = "16-18" },
              new ReservationTime { Hours = "18-20" },
              new ReservationTime { Hours = "20-22" }
            );


            //context.Users.AddOrUpdate(
            //  p => p.UserName,
            //  new User
            //  {
            //      UserName = "admin",
            //      UserEmail = "oidldb@gmail.com",
            //      UserPassword = "8ueosSTqRqEoeSO0qddj0u/8duNF2iee0b99LKOOlJgiHmN88FEUZ/Lre45kOqGD3Wl1iPKr/hSfWvBGkPNjvQ==",
            //      UserPasswordConfirm = "8ueosSTqRqEoeSO0qddj0u/8duNF2iee0b99LKOOlJgiHmN88FEUZ/Lre45kOqGD3Wl1iPKr/hSfWvBGkPNjvQ==",
            //      UserPasswordSalt = "100000.hCkNVNm/n2ElkChtHnS2me1VywTjxpjAoxnyYLLoWKisAg==",
            //      UserGroupID = context.UserGroups.Where(e => e.UserGroupName == "Administrator").FirstOrDefault().IDUserGroup,
            //      UserGroup = context.UserGroups.Where(e=>e.UserGroupName=="Administrator").FirstOrDefault()
            //      }
            //    );
        }
    }
}
