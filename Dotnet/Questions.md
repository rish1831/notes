# .NET Interview Question Bank

For **4 years .NET Backend / Full Stack experience**, these are the kinds of questions to expect in interviews.

## How To Use This File

1. Pick one section.
2. Answer each question aloud.
3. Open the topic note when you cannot answer confidently.
4. Mark weak topics for another pass.

## Questions

### C# / .NET

1. What happens internally when you call an async method?
2. How does async/await work under the hood?
3. Task vs Thread vs ThreadPool?
4. What is SynchronizationContext?
5. What is ConfigureAwait(false)?
6. What causes deadlocks in async code?
7. Task.WhenAll vs Parallel.ForEach?
8. Task.Run vs async I/O?
9. What is ValueTask?
10. How do CancellationTokens work?
11. What are delegates?
12. What are events?
13. Action vs Func vs Predicate?
14. What is covariance and contravariance?
15. What are extension methods?
16. What are attributes?
17. What is reflection?
18. What are expression trees?
19. What is boxing and unboxing?
20. Struct vs class?
21. Record vs class?
22. Abstract class vs interface?
23. Virtual vs abstract methods?
24. Sealed class?
25. What are nullable reference types?
26. What is the difference between == and Equals()?
27. GetHashCode() purpose?
28. IEquatable<T> usage?
29. IDisposable pattern?
30. Finalizer vs Dispose?

### CLR / Memory

31. What happens when you create an object?
32. Explain CLR execution flow.
33. What is IL code?
34. What is JIT compilation?
35. What is GC?
36. Gen0, Gen1, Gen2?
37. Large Object Heap?
38. What causes memory leaks in .NET?
39. Managed vs unmanaged memory?
40. How does connection pooling work?

### Dependency Injection

41. What problem does DI solve?
42. What is IoC?
43. Transient vs Scoped vs Singleton?
44. What happens if Singleton depends on Scoped?
45. How does IServiceProvider work?
46. How are services resolved?
47. Multiple implementations of same interface?
48. Open generics in DI?
49. Factory pattern with DI?
50. When would you avoid DI?

### ASP.NET Core

51. Request lifecycle in ASP.NET Core?
52. What is middleware?
53. Middleware execution order?
54. Custom middleware implementation?
55. Authentication vs Authorization?
56. JWT authentication flow?
57. Claims vs Roles?
58. What is CORS?
59. What is rate limiting?
60. Exception handling middleware?
61. Action filters?
62. Model binding?
63. API versioning approaches?
64. How do you secure APIs?
65. What causes 502/504 errors?

### SQL

66. Clustered vs Non-Clustered index?
67. Composite indexes?
68. Covering indexes?
69. Why index order matters?
70. Explain query execution plan.
71. Inner vs Left vs Right Join?
72. CTE vs Temp Table?
73. Window functions?
74. ROW_NUMBER vs RANK?
75. How would you find duplicates?
76. Isolation levels?
77. Deadlocks?
78. Optimistic vs Pessimistic locking?
79. Pagination strategies?
80. How do you optimize slow queries?

### Entity Framework

81. Tracking vs NoTracking?
82. What is DbContext?
83. Unit of Work in EF?
84. Repository pattern needed with EF?
85. Lazy vs Eager vs Explicit loading?
86. Include() vs Select()?
87. N+1 problem?
88. Migrations?
89. Compiled queries?
90. Bulk updates in EF?

### RabbitMQ / Messaging

91. Exchange types?
92. Direct vs Topic vs Fanout?
93. Routing Key vs Binding Key?
94. Durable queue?
95. Persistent messages?
96. Ack vs Nack?
97. Prefetch count?
98. DLQ?
99. How do duplicate messages happen?
100.  How do you handle duplicates?
101.  At-most-once vs At-least-once vs Exactly-once?
102.  Idempotency?
103.  Quorum Queue?
104.  RabbitMQ cluster?
105.  Consumer scaling?

### Redis

106. Why Redis?
107. Cache-aside pattern?
108. Distributed lock?
109. Redis expiry?
110. Redis Pub/Sub?
111. Redis persistence?
112. Cache invalidation strategies?

### System Design / HLD

113. Design a URL shortener.
114. Design a notification service.
115. Design a job application system.
116. Design interview scheduling system.
117. Design rate limiter.
118. Design chat system.
119. Design file upload service.
120. Design a leaderboard.
121. Horizontal vs vertical scaling?
122. Load balancer types?
123. CDN?
124. Database sharding?
125. Read replicas?

### Microservices

126. Monolith vs microservices?
127. API Gateway?
128. Service discovery?
129. Distributed transactions?
130. Saga pattern?
131. Circuit breaker?
132. Retry policies?
133. Bulkhead pattern?
134. Event-driven architecture?
135. Outbox pattern?

### Docker / Kubernetes

136. What is a Docker image?
137. Image vs Container?
138. Multi-stage builds?
139. Docker networking?
140. Kubernetes pod?
141. Deployment vs StatefulSet?
142. ConfigMap vs Secret?
143. Readiness probe?
144. Liveness probe?
145. HPA?
146. Rolling deployment?
147. Blue-Green deployment?

### AWS / Cloud

148. Lambda?
149. SQS?
150. SNS vs SQS?
151. API Gateway?
152. S3?
153. CloudWatch?
154. IAM roles?
155. ECS vs EKS?
156. Why use serverless?

### Experience-Based Questions

157. Biggest production issue you handled?
158. How did you debug a memory issue?
159. Slow API investigation?
160. RabbitMQ outage handling?
161. Redis outage handling?
162. How did you improve performance?
163. Most challenging feature you built?
164. How do you monitor production systems?
165. How do you handle duplicate interview bookings?
166. Explain one microservice you own end-to-end.

These 166 questions cover ~90% of what you'd face for a **₹30–35 LPA Senior Software Engineer / SDE2 .NET interview**.
