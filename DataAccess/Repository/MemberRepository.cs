using BusinessObject.Entity;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public void Add(Member member)
        {
            MemberDAO.Instance.AddMember(member);
        }

        public void Delete(Member member)
        {
            MemberDAO.Instance.DeleteMember(member);
        }

        public Member FindByEmail(string email)
        {
            return MemberDAO.Instance.FindOne(member => member.Email == email);
        }

       

        public Member FindById(int id)
        {
            return MemberDAO.Instance.FindOne(member => member.MemberId == id);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return MemberDAO.Instance.GetMembers();
        }

        public int Login(string email, string password)
        {
           return MemberDAO.Instance.CheckLogin(email, password);
        }

        public void Update(Member member)
        {
            MemberDAO.Instance.UpdateMember(member);
        }
    }
}
