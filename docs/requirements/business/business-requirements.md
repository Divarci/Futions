# Business Requirements Document (BRD)

**Product:** Todo Backend System (API-first)  
**Version:** v1  

---

## 1. Purpose

This system provides a backend service that enables users to create, organize, and track tasks.

**Objectives:**
- Centralize task management  
- Provide a simple but extensible business model  

---

## 2. Scope

### In Scope
- Task creation, update, deletion, listing  
- Tag assignment to tasks  
- Due date assignment  
- Task filtering (status, tag, due date)  

### Out of Scope (v1)
- Authentication / user management  
- Notification system (data only)  
- Team collaboration  
- UI (Web/Mobile)  

---

## 3. Stakeholders

- Product Owner  
- Backend Developer  
- QA  

---

## 4. Key Concepts

### Task
Represents a unit of work.

### Tag
Used to categorize tasks.

### Due Date
Expected completion date of a task.

---

## 5. Goals & Objectives

- Enable structured task tracking  
- Allow categorization via tags  
- Support time-based tracking  

---

## 6. Use Cases

### UC-01: Create Task
User creates a task.  
- Default status: Pending  

### UC-02: Update Task
User updates task details.

### UC-03: Delete Task
User deletes a task (permanent).

### UC-04: List Tasks
User views all tasks with filters.

### UC-05: Assign Tags
User assigns tags to tasks.

### UC-06: Filter by Tag
User filters tasks by tag.

### UC-07: Set Due Date
User assigns a due date.

### UC-08: View Upcoming Tasks
User views tasks within a time range.  
- Only pending tasks included  

---

## 7. Functional Requirements

### Task Management
- Create, update, delete, list tasks  

### Tag Management
- Create, list, delete tags  

### Filtering
- By status  
- By tag  
- By due date range  

---

## 8. Business Rules

- Task title is required  
- Status must be:
  - Pending  
  - Completed  

- A task:
  - Can have multiple tags  
  - Can have zero tags  

- Tag name:
  - Cannot be empty  
  - Must be unique  

- Due Date:
  - Optional  
  - Can be in the past  

- Completed tasks:
  - Not included in upcoming list  

- Deleted tasks:
  - Cannot be recovered  

---

## 9. Non-Functional Requirements

### Performance
- Task listing should be fast  

### Usability
- Operations should be minimal-step  

### Reliability
- No data loss  

---

## 10. Success Metrics

- Number of created tasks  
- Task completion rate  
- Percentage of tasks with due date  
- Percentage of tasks using tags  

---

## 11. Assumptions

- Single-user system initially  
- Backend manages all data  
- Interaction via API  

---

## 12. Future Enhancements

- Authentication / multi-user  
- Notifications  
- Task sharing  
- AI suggestions  
