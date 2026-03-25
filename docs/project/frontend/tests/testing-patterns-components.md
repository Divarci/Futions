# Testing — Component Patterns

Mock the hook, not the API. Components must never reach real infra in tests.

---

## Pattern

```typescript
// features/tasks/components/TaskList.test.tsx
import { render, screen } from "@testing-library/react";
import userEvent          from "@testing-library/user-event";
import { useGetTasks }    from "../hooks/useGetTasks";
import { TaskList }       from "./TaskList";

vi.mock("../hooks/useGetTasks");

describe("TaskList", () => {

    it("renders loading state", () => {
        // Arrange
        vi.mocked(useGetTasks).mockReturnValue({
            data:      undefined,
            isLoading: true,
            error:     undefined,
        } as ReturnType<typeof useGetTasks>);

        // Act
        render(<TaskList filter={{}} />);

        // Assert
        expect(screen.getByRole("status")).toBeInTheDocument();
    });

    it("renders error state", () => {
        // Arrange
        vi.mocked(useGetTasks).mockReturnValue({
            data:      undefined,
            isLoading: false,
            error:     new Error("Failed"),
        } as ReturnType<typeof useGetTasks>);

        // Act
        render(<TaskList filter={{}} />);

        // Assert
        expect(screen.getByText(/something went wrong/i)).toBeInTheDocument();
    });

    it("renders tasks when data is available", () => {
        // Arrange
        const tasks = [{ taskId: "1", title: "My Task" }];
        vi.mocked(useGetTasks).mockReturnValue({
            data:      tasks,
            isLoading: false,
            error:     undefined,
        } as ReturnType<typeof useGetTasks>);

        // Act
        render(<TaskList filter={{}} />);

        // Assert
        expect(screen.getByText("My Task")).toBeInTheDocument();
    });
});
```

---

## User Interaction

```typescript
it("calls onDelete when delete button is clicked", async () => {
    // Arrange
    const onDelete = vi.fn();
    const task     = { taskId: "1", title: "My Task" };
    const user     = userEvent.setup();

    render(<TaskCard task={task} onDelete={onDelete} />);

    // Act
    await user.click(screen.getByRole("button", { name: /delete/i }));

    // Assert
    expect(onDelete).toHaveBeenCalledWith("1");
});
```

---

## Rules

- Always mock the consuming hook — never let the component reach `infra/`.
- Test all three rendering states: loading, error, data.
- Use `screen.getByRole` and `screen.getByText` — never query by CSS class.
- Use `userEvent.setup()` for user interactions — not `fireEvent`.
