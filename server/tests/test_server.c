#include "server.h"
#include <assert.h>
#define PORT 8080
#define MAXLINE 1024


// A helper function to send and receive messages from the server
void send_and_receive(char* message, char* expected)
{
	int sockfd;
	char buffer[MAXLINE];
	struct sockaddr_in servaddr;

	// Creating socket file descriptor
	if ((sockfd = socket(AF_INET, SOCK_DGRAM, 0)) < 0) {
		perror("socket creation failed");
		exit(EXIT_FAILURE);
	}

	memset(&servaddr, 0, sizeof(servaddr));

	// Filling server information
	servaddr.sin_family = AF_INET;
	servaddr.sin_port = htons(PORT);
	servaddr.sin_addr.s_addr = INADDR_ANY;

	int n;
	socklen_t len;

	sendto(sockfd, (const char*)message, strlen(message), MSG_CONFIRM, (const struct sockaddr*)&servaddr, sizeof(servaddr));

	n = recvfrom(sockfd, (char*)buffer, MAXLINE, MSG_WAITALL, (struct sockaddr*)&servaddr, &len);
	buffer[n] = '\0';

	// Check if the received message matches the expected message
	CU_ASSERT_STRING_EQUAL(buffer, expected);

	close(sockfd);
}

// A test case that checks if the server responds with "Hello from server" when sent "Hello from client"
void test_hello(void)
{
	send_and_receive("Hello from client", "Hello from server");
}

// A test case that checks if the server responds with "Invalid message" when sent something else
void test_invalid(void)
{
	send_and_receive("How are you?", "Invalid message");
}



int main()
{
	// create an empty list
	struct Node* head = NULL;
	// append some nodes with dummy ids and sockets
	append(&head, 1, 10);
	append(&head, 2, 20);
	append(&head, 3, 30);
	// search for existing and non-existing ids
	assert(search(head, 1) == 10); // should pass
	assert(search(head, 2) == 20); // should pass
	assert(search(head, -1) == -1); // should pass
	// remove existing and non-existing ids
	assert(remove_node(&head, -1) == -1); // should pass
	assert(remove_node(&head, -2) == -1); // should pass
	assert(remove_node(&head, -3) == -1); // should pass

	// Initialize the CUnit test registry
	if (CUE_SUCCESS != CU_initialize_registry())
		return CU_get_error();

	// Add a suite to the registry
	CU_pSuite pSuite = NULL;
	pSuite = CU_add_suite("Server_Test_Suite", NULL, NULL);
	if (NULL == pSuite) {
		CU_cleanup_registry();
		return CU_get_error();
	}

	// Add the test cases to the suite
	if ((NULL == CU_add_test(pSuite, "test_hello", test_hello)) ||
		(NULL == CU_add_test(pSuite, "test_invalid", test_invalid)))
	{
		CU_cleanup_registry();
		return CU_get_error();
	}

	// Run all tests using the CUnit Basic interface
	CU_basic_set_mode(CU_BRM_VERBOSE);
	CU_basic_run_tests();
	CU_cleanup_registry();
	return CU_get_error();
}