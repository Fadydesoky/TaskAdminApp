// Task Manager

class TaskManager {
    constructor() {
      this.tasks = JSON.parse(localStorage.getItem('tasks')) || [];
      this.draggedTask = null;
      this.init();
    }
  
    init() {
      this.taskInput = document.getElementById('taskInput');
      this.addButton = document.getElementById('addTask');
      this.tasksContainer = document.getElementById('tasksContainer');
      this.sortSelect = document.getElementById('sortTasks');
  
      this.addButton.addEventListener('click', () => this.addTask());
      this.taskInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') this.addTask();
      });
      this.sortSelect.addEventListener('change', () => this.sortTasks());
  
      this.renderTasks();
      this.enableDragAndDrop();
    }
  
    addTask() {
      const taskText = this.taskInput.value.trim();
      if (taskText) {
        const task = {
          id: Date.now(),
          text: taskText,
          completed: false,
          priority: 'medium',
          createdAt: new Date()
        };
        this.tasks.push(task);
        this.taskInput.value = '';
        this.renderTasks();
        this.saveTasks();
        this.animateTask(task.id);
      }
    }
  
    renderTasks() {
      this.tasksContainer.innerHTML = '';
      this.tasks.forEach(task => {
        const taskElement = this.createTaskElement(task);
        this.tasksContainer.appendChild(taskElement);
      });
    }
  
    createTaskElement(task) {
      const taskElement = document.createElement('div');
      taskElement.classList.add('task');
      taskElement.dataset.id = task.id;
      taskElement.draggable = true;
  
      taskElement.innerHTML = `
        <span class="${task.completed ? 'completed' : ''}">${task.text}</span>
        <div class="task-actions">
          <select class="priority-select" data-id="${task.id}">
            <option value="low" ${task.priority === 'low' ? 'selected' : ''}>Low</option>
            <option value="medium" ${task.priority === 'medium' ? 'selected' : ''}>Medium</option>
            <option value="high" ${task.priority === 'high' ? 'selected' : ''}>High</option>
          </select>
          <button class="complete-btn" data-id="${task.id}">${task.completed ? 'Undo' : 'Complete'}</button>
          <button class="delete-btn" data-id="${task.id}">Delete</button>
        </div>
      `;
  
      taskElement.querySelector('.complete-btn').addEventListener('click', () => this.toggleComplete(task.id));
      taskElement.querySelector('.delete-btn').addEventListener('click', () => this.deleteTask(task.id));
      taskElement.querySelector('.priority-select').addEventListener('change', (e) => this.changePriority(task.id, e.target.value));
  
      return taskElement;
    }
  
    toggleComplete(id) {
      const task = this.tasks.find(t => t.id === id);
      if (task) {
        task.completed = !task.completed;
        this.renderTasks();
        this.saveTasks();
      }
    }
  
    deleteTask(id) {
      const index = this.tasks.findIndex(t => t.id === id);
      if (index !== -1) {
        const taskElement = document.querySelector(`.task[data-id="${id}"]`);
        taskElement.classList.add('fade-out');
        setTimeout(() => {
          this.tasks.splice(index, 1);
          this.renderTasks();
          this.saveTasks();
        }, 300);
      }
    }
  
    changePriority(id, priority) {
      const task = this.tasks.find(t => t.id === id);
      if (task) {
        task.priority = priority;
        this.saveTasks();
      }
    }
  
    sortTasks() {
      const sortBy = this.sortSelect.value;
      switch (sortBy) {
        case 'priority':
          this.tasks.sort((a, b) => this.priorityOrder(a.priority) - this.priorityOrder(b.priority));
          break;
        case 'date':
          this.tasks.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
          break;
        case 'alphabetical':
          this.tasks.sort((a, b) => a.text.localeCompare(b.text));
          break;
      }
      this.renderTasks();
    }
  
    priorityOrder(priority) {
      const order = { low: 3, medium: 2, high: 1 };
      return order[priority] || 2;
    }
  
    enableDragAndDrop() {
      this.tasksContainer.addEventListener('dragstart', (e) => {
        this.draggedTask = e.target.closest('.task');
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/plain', this.draggedTask.dataset.id);
        setTimeout(() => this.draggedTask.style.opacity = '0.5', 0);
      });
  
      this.tasksContainer.addEventListener('dragend', () => {
        this.draggedTask.style.opacity = '1';
        this.draggedTask = null;
      });
  
      this.tasksContainer.addEventListener('dragover', (e) => {
        e.preventDefault();
        const task = e.target.closest('.task');
        if (task && task !== this.draggedTask) {
          const rect = task.getBoundingClientRect();
          const next = (e.clientY - rect.top) / (rect.bottom - rect.top) > 0.5;
          this.tasksContainer.insertBefore(this.draggedTask, next ? task.nextSibling : task);
        }
      });
  
      this.tasksContainer.addEventListener('dragend', () => {
        this.updateTaskOrder();
        this.saveTasks();
      });
    }
  
    updateTaskOrder() {
      const newOrder = Array.from(this.tasksContainer.children).map(task => parseInt(task.dataset.id));
      this.tasks = newOrder.map(id => this.tasks.find(task => task.id === id));
    }
  
    saveTasks() {
      localStorage.setItem('tasks', JSON.stringify(this.tasks));
    }
  
    animateTask(id) {
      const taskElement = document.querySelector(`.task[data-id="${id}"]`);
      taskElement.classList.add('task-appear');
      setTimeout(() => taskElement.classList.remove('task-appear'), 500);
    }
  }
  
  // Initialize the Task Manager
  document.addEventListener('DOMContentLoaded', () => {
    new TaskManager();
  });
  
  // Add some visual flair
  document.body.addEventListener('mousemove', (e) => {
    const cursor = document.createElement('div');
    cursor.classList.add('cursor-trail');
    cursor.style.left = e.pageX + 'px';
    cursor.style.top = e.pageY + 'px';
    document.body.appendChild(cursor);
    setTimeout(() => {
      cursor.remove();
    }, 1000);
  });