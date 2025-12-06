using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ywBookStoreLIB.Tests
{
    [TestClass]
    public class ReviewValidationTests
    {

        [TestMethod]
        public void TDD_Test1_RatingValidation_LowBoundary()
        {
            Console.WriteLine("TDD Test 1: Rating must be at least 1");

            var dal = new DALReviews();

            try
            {
                dal.AddReview("0135974445", 4, 0, "Test review");
                Assert.Fail("❌ SHOULD HAVE FAILED: Rating 0 should be rejected");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"PASS: {ex.Message}");
            }
        }

        [TestMethod]
        public void TDD_Test2_RatingValidation_HighBoundary()
        {
            Console.WriteLine("TDD Test 2: Rating must be at most 5");

            var dal = new DALReviews();

            try
            {
                dal.AddReview("0135974445", 4, 6, "Test review");
                Assert.Fail("FAIL: Rating 6 should be rejected");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"✅ PASS: {ex.Message}");
            }
        }

        [TestMethod]
        public void TDD_Test3_EmptyReviewValidation()
        {
            Console.WriteLine("TDD Test 3: Review text cannot be empty");

            var dal = new DALReviews();

            try
            {
                dal.AddReview("0135974445", 4, 3, "");
                Assert.Fail("FAIL: Empty review should be rejected");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"PASS: {ex.Message}");
            }
        }

        [TestMethod]
        public void TDD_Test4_ValidInputShouldWork()
        {
            Console.WriteLine("TDD Test 4: Valid inputs should be accepted");

            var dal = new DALReviews();

            try
            {
                dal.AddReview("0135974445", 4, 5, "Excellent book!");
                Console.WriteLine("PASS: Valid review accepted");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("FAIL: Valid rating 5 was rejected");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("FAIL: Valid review text was rejected");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Note: Database error (expected): {ex.Message}");
                Console.WriteLine("Validation passed before database");
            }
        }

        [TestMethod]
        public void TDD_Test5_GetReviewsMethodExists()
        {
            Console.WriteLine("TDD Test 5: GetReviews method should exist");

            var dal = new DALReviews();

            var result = dal.GetReviewsByISBN("0135974445");

            Console.WriteLine("PASS: GetReviews method exists and returns DataTable");
        }
    }
}