using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Utilities
{
    public class EnumUtility
    {
        public enum UserCategoty
        {
            Facilitator = 1,
            SchoolUsers = 2,
            Learners = 3,
            Students = 4,
            Parents = 5,
            SystemUser = 6
        }

        //School Roles
        public enum SchoolRoles
        {
            SuperAdministrator = 1,
            Administrator = 2,
            ClassTeacher = 3,
            SubjectTeacher = 4,
        }
        public enum SystemRoles
        {
            Super_Administrator = 1,
            Administrator = 2,
            Content_Creator = 3
        }
        public enum FacilitatorType
        {
            External = 1,
            Internal = 2
        }

        //Months
        public enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        //Question Types
        public enum QuestionTypes
        {
            Multiple_Choice = 1,
            Fill_in_the_Gap = 2,
            True_or_False = 3
        }

        //Gender
        public enum Gender
        {
            Male = 1,
            Female = 2,
        }

        //Class Or Alumni
        public enum ClassOrAlumni
        {
            Alumni = 1,
            Class = 2,
        }

        //Attendance Periods
        public enum AttendancePeriod
        {
            Morning = 1,
            Afternoon = 2,
            Both = 3,
        }

        //Status 
        public enum Status
        {
            Approved = 1,
            Pending = 2,
            Declined = 3,
        }

        //ScoreStatus 
        public enum ScoreStatus
        {
            Passed = 1,
            Failed = 2,
            Pending = 3,
        }

        //ScoreCategory 
        public enum ScoreCategory
        {
            Exam = 1,
            CA = 2,
            Behavioural = 3,
            ExtraCurricular = 4,
        }

        //Active/InActive Status 
        public enum ActiveInActive
        {
            Active = 1,
            InActive = 2,
        }

        //Active/InActive Status 
        public enum SchoolSubTypes
        {
            Junior = 1,
            Senior = 2,
            Primary = 3,
            Nursery = 4,
        }
        //Payment Method
        public enum PaymentMethod
        {
            Bank_Deposit = 1,
            Online_Transfer = 2,
            Card_Payment = 3
        }

        public enum Terms
        {
            FirstTerm = 1,
            SecondTerm = 2,
            ThirdTerm = 3,
        }

        public enum CommentConfig
        {
            Examiner = 1,
            ClassTeacher = 2,
            HeadTeacher = 3,
            Principal = 4,
        }

    }
}
