using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MemberDAO
    {
        public static MemberDAO instance;
        private static readonly object instanceLock = new object();

        private MemberDAO()
        {
        }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public int CheckLogin(string email, string password)
        {
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    var member = salesDB.Members.Where(m => m.Email == email && m.Password == password).FirstOrDefault();
                    if (email.Equals(salesDB.AdminEmail) && password.Equals(salesDB.AdminPassword))
                    {   
                        return 0;    
                    }   
                    else if (member != null)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                   
                }   
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Member FindOne(Expression<Func<Member, bool>> predicate)
        {
            Member member = null;
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    member = salesDB.Members.SingleOrDefault(predicate);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return member;
        }

        public void AddMember(Member member)
        {
            try
            {
                Member mem = FindOne(item => item.MemberId.Equals(member.MemberId));

                if (mem == null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Members.Add(member);
                        salesDB.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public IEnumerable<Member> GetMembers()
        {
            List<Member> members = new List<Member>();
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    members = salesDB.Members.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return members;

        }

        public void UpdateMember(Member member)
        {
            try
            {
                Member mem = FindOne(item => item.MemberId.Equals(member.MemberId));
                if (mem != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Members.Update(member);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Member not found");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteMember(Member member)
        {
            try
            {
                Member mem = FindOne(item => item.MemberId.Equals(member.MemberId));
                if (mem != null)
                {
                    using (var salesDB = new As1storeContext())
                    {
                        salesDB.Members.Remove(member);
                        salesDB.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Member not found");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
