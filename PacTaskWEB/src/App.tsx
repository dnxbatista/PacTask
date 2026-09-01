import {
  Alert,
  AppShell,
  Badge,
  Button,
  Card,
  Checkbox,
  Container,
  Group,
  PasswordInput,
  Paper,
  SegmentedControl,
  Stack,
  Text,
  Textarea,
  TextInput,
  Title,
} from '@mantine/core'
import { useEffect, useState } from 'react'
import './App.css'

type UserSession = {
  username: string
  email: string
  token: string
}

type EnvironmentItem = {
  id: number
  title: string
}

type TaskItem = {
  id: number
  title: string
  description: string
  status: number
  environmentId: number
}

type AuthFormValues = {
  username: string
  email: string
  password: string
}

const STORAGE_KEY = 'pactask-session'

const getApiBases = () => {
  const configured = (import.meta.env.VITE_API_URL as string | undefined)?.trim() // if you have a .env file
  const bases = configured ? [configured] : []
  return [...new Set([...bases, 'http://localhost:5021', 'https://localhost:7169'])]
}

const parseJsonOrText = (raw: string) => {
  if (!raw) {
    return null
  }

  try {
    return JSON.parse(raw)
  } catch {
    return raw
  }
}

async function request<T>(path: string, options: RequestInit = {}, token?: string): Promise<T> {
  const headers = new Headers(options.headers ?? {})

  if (token) {
    headers.set('Authorization', `Bearer ${token}`)
  }

  if (options.body && typeof options.body === 'string' && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  const lastErrors: string[] = []

  for (const baseUrl of getApiBases()) {
    try {
      const response = await fetch(`${baseUrl}${path}`, {
        ...options,
        headers,
      })

      const raw = await response.text()
      const data = parseJsonOrText(raw)

      if (response.ok) {
        return data as T
      }

      const message =
        typeof data === 'string'
          ? data
          : data?.message ?? data?.title ?? `Request failed with status ${response.status}`

      throw new Error(message)
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error)
      lastErrors.push(message)

      const isNetworkFailure =
        message.includes('Failed to fetch') ||
        message.includes('NetworkError') ||
        message.includes('TypeError')

      if (isNetworkFailure) {
        continue
      }

      throw error
    }
  }

  throw new Error(lastErrors.at(-1) ?? 'Unable to reach the PacTask API.')
}

const saveSession = (session: UserSession | null) => {
  if (!session) {
    localStorage.removeItem(STORAGE_KEY)
    return
  }

  localStorage.setItem(STORAGE_KEY, JSON.stringify(session))
}

const loadSession = (): UserSession | null => {
  const raw = localStorage.getItem(STORAGE_KEY)

  if (!raw) {
    return null
  }

  try {
    const parsed = JSON.parse(raw) as UserSession
    if (!parsed?.token) {
      return null
    }

    return parsed
  } catch {
    localStorage.removeItem(STORAGE_KEY)
    return null
  }
}

const authRequest = async (path: string, payload: Record<string, string>) =>
  request<{ username: string; email: string; token: string }>(path, {
    method: 'POST',
    body: JSON.stringify(payload),
  })

const getEnvironments = async (token: string) =>
  request<EnvironmentItem[]>('/api/Environment', { method: 'GET' }, token)

const createEnvironment = async (title: string, token: string) =>
  request<EnvironmentItem>('/api/Environment', { method: 'POST', body: JSON.stringify({ title }) }, token)

const updateEnvironment = async (id: number, title: string, token: string) =>
  request<EnvironmentItem>(`/api/Environment/${id}`, { method: 'PUT', body: JSON.stringify({ title }) }, token)

const deleteEnvironment = async (id: number, token: string) =>
  request<EnvironmentItem>(`/api/Environment/${id}`, { method: 'DELETE' }, token)

const getTasks = async (environmentId: number, token: string) =>
  request<TaskItem[]>(`/api/Task/${environmentId}`, { method: 'GET' }, token)

const createTask = async (environmentId: number, title: string, description: string, token: string) =>
  request<TaskItem>(`/api/Task/${environmentId}`, { method: 'POST', body: JSON.stringify({ title, description }) }, token)

const updateTask = async (taskId: number, title: string, description: string, status: number, token: string) =>
  request<TaskItem>(`/api/Task/${taskId}`, { method: 'PUT', body: JSON.stringify({ title, description, status }) }, token)

const deleteTask = async (taskId: number, token: string) =>
  request<TaskItem>(`/api/Task/${taskId}`, { method: 'DELETE' }, token)

function App() {
  const [session, setSession] = useState<UserSession | null>(() => loadSession())
  const [authMode, setAuthMode] = useState<'login' | 'register'>('login')
  const [authForm, setAuthForm] = useState<AuthFormValues>({ username: '', email: '', password: '' })
  const [authError, setAuthError] = useState('')
  const [isSubmittingAuth, setIsSubmittingAuth] = useState(false)

  const [environments, setEnvironments] = useState<EnvironmentItem[]>([])
  const [selectedEnvironmentId, setSelectedEnvironmentId] = useState<number | null>(null)
  const [tasks, setTasks] = useState<TaskItem[]>([])
  const [environmentDraft, setEnvironmentDraft] = useState('')
  const [editingEnvironmentId, setEditingEnvironmentId] = useState<number | null>(null)
  const [taskDraft, setTaskDraft] = useState({ title: '', description: '' })
  const [editingTaskId, setEditingTaskId] = useState<number | null>(null)
  const [taskError, setTaskError] = useState('')
  const [isSavingTask, setIsSavingTask] = useState(false)
  const [isSavingEnvironment, setIsSavingEnvironment] = useState(false)

  const selectedEnvironment = environments.find((environment) => environment.id === selectedEnvironmentId) ?? null

  const loadEnvironments = async (token: string) => {
    try {
      const nextEnvironments = await getEnvironments(token)
      setEnvironments(nextEnvironments)

      if (!selectedEnvironmentId && nextEnvironments.length > 0) {
        setSelectedEnvironmentId(nextEnvironments[0].id)
      }

      if (selectedEnvironmentId !== null && !nextEnvironments.some((environment) => environment.id === selectedEnvironmentId)) {
        setSelectedEnvironmentId(nextEnvironments[0]?.id ?? null)
      }
    } catch (error) {
      setAuthError(error instanceof Error ? error.message : 'Unable to load environments.')
    }
  }

  const loadTasks = async (environmentId: number, token: string) => {
    try {
      const nextTasks = await getTasks(environmentId, token)
      setTasks(nextTasks)
    } catch (error) {
      setTaskError(error instanceof Error ? error.message : 'Unable to load tasks.')
    }
  }

  useEffect(() => {
    if (!session) {
      setEnvironments([])
      setSelectedEnvironmentId(null)
      setTasks([])
      return
    }

    void loadEnvironments(session.token)
  }, [session])

  useEffect(() => {
    if (!session || selectedEnvironmentId === null) {
      setTasks([])
      return
    }

    void loadTasks(selectedEnvironmentId, session.token)
  }, [selectedEnvironmentId, session])

  const handleAuthSubmit = async () => {
    setAuthError('')
    setIsSubmittingAuth(true)

    try {
      const endpoint = authMode === 'login' ? '/api/User' : '/api/User/register'
      const payload = authMode === 'login'
        ? { username: authForm.username, email: authForm.email, password: authForm.password }
        : { username: authForm.username, email: authForm.email, password: authForm.password }

      const loggedUser = await authRequest(endpoint, payload)
      const nextSession: UserSession = {
        username: loggedUser.username,
        email: loggedUser.email,
        token: loggedUser.token,
      }

      setSession(nextSession)
      saveSession(nextSession)
      setAuthForm({ username: '', email: '', password: '' })
    } catch (error) {
      const message = error instanceof Error ? error.message : 'An unexpected error happened.'
      setAuthError(message)
    } finally {
      setIsSubmittingAuth(false)
    }
  }

  const handleLogout = () => {
    setSession(null)
    saveSession(null)
  }

  const handleCreateOrUpdateEnvironment = async () => {
    if (!session) {
      return
    }

    const trimmed = environmentDraft.trim()
    if (!trimmed) {
      setAuthError('Environment title is required.')
      return
    }

    setIsSavingEnvironment(true)
    setAuthError('')

    try {
      if (editingEnvironmentId !== null) {
        const updated = await updateEnvironment(editingEnvironmentId, trimmed, session.token)
        setEnvironments((current) =>
          current.map((environment) => (environment.id === updated.id ? { ...environment, title: updated.title } : environment)),
        )
      } else {
        // The API currently returns only the create payload (`{ title }`), not
        // the persisted environment DTO. Reload the collection to obtain its id.
        await createEnvironment(trimmed, session.token)
        const refreshedEnvironments = await getEnvironments(session.token)
        setEnvironments(refreshedEnvironments)
        const createdEnvironment = refreshedEnvironments.find((environment) => environment.title === trimmed)
        setSelectedEnvironmentId(createdEnvironment?.id ?? refreshedEnvironments[0]?.id ?? null)
      }

      setEnvironmentDraft('')
      setEditingEnvironmentId(null)
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unable to save environment.'
      setAuthError(message)
    } finally {
      setIsSavingEnvironment(false)
    }
  }

  const handleDeleteEnvironment = async (environmentId: number) => {
    if (!session) {
      return
    }

    const confirmed = window.confirm('Delete this environment and all its tasks?')
    if (!confirmed) {
      return
    }

    try {
      await deleteEnvironment(environmentId, session.token)
      const nextEnvironments = environments.filter((environment) => environment.id !== environmentId)
      setEnvironments(nextEnvironments)

      if (selectedEnvironmentId === environmentId) {
        setSelectedEnvironmentId(nextEnvironments[0]?.id ?? null)
      }
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unable to delete environment.'
      setAuthError(message)
    }
  }

  const handleTaskSubmit = async () => {
    if (!session || selectedEnvironmentId === null) {
      return
    }

    const title = taskDraft.title.trim()
    const description = taskDraft.description.trim()

    if (!title || !description) {
      setTaskError('Title and description are required.')
      return
    }

    setIsSavingTask(true)
    setTaskError('')

    try {
      if (editingTaskId !== null) {
        const currentTask = tasks.find((task) => task.id === editingTaskId)
        const updated = await updateTask(
          editingTaskId,
          title,
          description,
          currentTask?.status ?? 0,
          session.token,
        )

        setTasks((current) => current.map((task) => (task.id === updated.id ? updated : task)))
      } else {
        // The API returns only the create payload (`{ title, description }`),
        // so reload the collection to obtain the persisted task id.
        await createTask(selectedEnvironmentId, title, description, session.token)
        const refreshedTasks = await getTasks(selectedEnvironmentId, session.token)
        setTasks(refreshedTasks)
      }

      setTaskDraft({ title: '', description: '' })
      setEditingTaskId(null)
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unable to save task.'
      setTaskError(message)
    } finally {
      setIsSavingTask(false)
    }
  }

  const handleDeleteTask = async (taskId: number) => {
    if (!session) {
      return
    }

    const confirmed = window.confirm('Delete this task?')
    if (!confirmed) {
      return
    }

    try {
      await deleteTask(taskId, session.token)
      setTasks((current) => current.filter((task) => task.id !== taskId))
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unable to delete task.'
      setTaskError(message)
    }
  }

  const handleToggleTask = async (task: TaskItem) => {
    if (!session) {
      return
    }

    const nextStatus = task.status === 1 ? 0 : 1

    try {
      const updated = await updateTask(task.id, task.title, task.description, nextStatus, session.token)
      setTasks((current) => current.map((item) => (item.id === updated.id ? updated : item)))
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unable to update task status.'
      setTaskError(message)
    }
  }

  const beginTaskEdit = (task: TaskItem) => {
    setEditingTaskId(task.id)
    setTaskDraft({ title: task.title, description: task.description })
  }

  if (!session) {
    return (
      <div className="auth-page">
        <Container size="xs">
          <Paper radius="lg" p="xl" className="auth-card" withBorder>
            <Stack gap="lg">
              <div>
                <Title order={2}>PacTask</Title>
                <Text c="dimmed">Organize your work by environment and task.</Text>
              </div>

              <SegmentedControl
                value={authMode}
                onChange={(value) => setAuthMode(value as 'login' | 'register')}
                data={[
                  { label: 'Login', value: 'login' },
                  { label: 'Register', value: 'register' },
                ]}
              />

              <TextInput
                label="Username"
                placeholder="yourname"
                value={authForm.username}
                onChange={(event) => {
                  const username = event.currentTarget.value
                  setAuthForm((current) => ({ ...current, username }))
                }}
              />

              <TextInput
                label="Email"
                placeholder="name@example.com"
                type="email"
                value={authForm.email}
                onChange={(event) => {
                  const email = event.currentTarget.value
                  setAuthForm((current) => ({ ...current, email }))
                }}
              />

              <PasswordInput
                label="Password"
                placeholder="Enter your password"
                value={authForm.password}
                onChange={(event) => {
                  const password = event.currentTarget.value
                  setAuthForm((current) => ({ ...current, password }))
                }}
              />

              {authError ? <Alert color="red">{authError}</Alert> : null}

              <Button loading={isSubmittingAuth} onClick={handleAuthSubmit}>
                {authMode === 'login' ? 'Login' : 'Create account'}
              </Button>
            </Stack>
          </Paper>
        </Container>
      </div>
    )
  }

  return (
    <AppShell
      navbar={{
        width: 300,
        breakpoint: 'sm',
      }}
      padding="md"
    >
      <AppShell.Navbar p="md">
        <Stack gap="lg">
          <div>
            <Title order={3}>PacTask</Title>
            <Text size="sm" c="dimmed">Welcome, {session.username}</Text>
          </div>

          <Stack gap="xs">
            <Text fw={600}>Create environment</Text>
            <TextInput
              placeholder="Work, Home, Study..."
              value={environmentDraft}
              onChange={(event) => setEnvironmentDraft(event.currentTarget.value)}
            />
            <Button loading={isSavingEnvironment} onClick={handleCreateOrUpdateEnvironment}>
              {editingEnvironmentId !== null ? 'Save changes' : 'Add environment'}
            </Button>
            {editingEnvironmentId !== null ? (
              <Button variant="subtle" onClick={() => {
                setEditingEnvironmentId(null)
                setEnvironmentDraft('')
              }}>
                Cancel
              </Button>
            ) : null}
          </Stack>

          <Stack gap="xs">
            {environments.length === 0 ? (
              <Text c="dimmed">No environments yet.</Text>
            ) : (
              environments.map((environment) => (
                <Card key={environment.id} p="sm" withBorder className={selectedEnvironmentId === environment.id ? 'selected-card' : ''}>
                  <Group justify="space-between" wrap="nowrap">
                    <Button
                      variant={selectedEnvironmentId === environment.id ? 'filled' : 'subtle'}
                      fullWidth
                      className="environment-title-button"
                      justify="flex-start"
                      onClick={() => setSelectedEnvironmentId(environment.id)}
                    >
                      {environment.title}
                    </Button>
                    <Button
                      variant="subtle"
                      size="compact-sm"
                      className="environment-action-button"
                      onClick={() => {
                        setEditingEnvironmentId(environment.id)
                        setEnvironmentDraft(environment.title)
                      }}
                    >
                      Edit
                    </Button>
                    <Button
                      variant="subtle"
                      color="red"
                      size="compact-sm"
                      className="environment-action-button"
                      onClick={() => void handleDeleteEnvironment(environment.id)}
                    >
                      Delete
                    </Button>
                  </Group>
                </Card>
              ))
            )}
          </Stack>
        </Stack>
      </AppShell.Navbar>

      <AppShell.Main>
        <Container size="lg" py="md">
          <Group justify="space-between" align="center" mb="lg">
            <div>
              <Title order={2}>{selectedEnvironment?.title ?? 'No environment selected'}</Title>
              <Text c="dimmed">{tasks.length} tasks in this list</Text>
            </div>

            <Button color="red" variant="light" onClick={handleLogout}>
              Logout
            </Button>
          </Group>

          {selectedEnvironment === null ? (
            <Paper p="xl" withBorder radius="md">
              <Text>Create or select an environment to begin managing tasks.</Text>
            </Paper>
          ) : (
            <Stack gap="lg">
              <Paper p="lg" radius="md" withBorder>
                <Stack gap="sm">
                  <Text fw={600}>New task</Text>
                  <TextInput
                    label="Task title"
                    placeholder="Ship the landing page"
                    value={taskDraft.title}
                    onChange={(event) => {
                      const title = event.currentTarget.value
                      setTaskDraft((current) => ({ ...current, title }))
                    }}
                  />
                  <Textarea
                    label="Description"
                    placeholder="Write a clear summary of what needs to be done..."
                    minRows={3}
                    value={taskDraft.description}
                    onChange={(event) => {
                      const description = event.currentTarget.value
                      setTaskDraft((current) => ({ ...current, description }))
                    }}
                  />
                  {taskError ? <Alert color="red">{taskError}</Alert> : null}
                  <Group>
                    <Button loading={isSavingTask} onClick={() => void handleTaskSubmit()}>
                      {editingTaskId !== null ? 'Save task' : 'Add task'}
                    </Button>
                    {editingTaskId !== null ? (
                      <Button
                        variant="subtle"
                        onClick={() => {
                          setEditingTaskId(null)
                          setTaskDraft({ title: '', description: '' })
                        }}
                      >
                        Cancel
                      </Button>
                    ) : null}
                  </Group>
                </Stack>
              </Paper>

              {tasks.length === 0 ? (
                <Paper p="xl" radius="md" withBorder>
                  <Text c="dimmed">No tasks here yet. Add the first one above.</Text>
                </Paper>
              ) : (
                <Stack gap="sm">
                  {tasks.map((task) => (
                    <Card key={task.id} p="md" withBorder>
                      <Group justify="space-between" align="flex-start" wrap="nowrap">
                        <div>
                          <Group gap="xs">
                            <Text fw={600}>{task.title}</Text>
                            <Badge color={task.status === 1 ? 'green' : 'yellow'}>
                              {task.status === 1 ? 'Done' : 'Pending'}
                            </Badge>
                          </Group>
                          <Text mt="xs" c="dimmed" size="sm">
                            {task.description}
                          </Text>
                        </div>

                        <Group>
                          <Checkbox
                            label={task.status === 1 ? 'Completed' : 'Mark done'}
                            checked={task.status === 1}
                            onChange={() => void handleToggleTask(task)}
                          />
                          <Button variant="subtle" onClick={() => beginTaskEdit(task)}>
                            Edit
                          </Button>
                          <Button color="red" variant="subtle" onClick={() => void handleDeleteTask(task.id)}>
                            Delete
                          </Button>
                        </Group>
                      </Group>
                    </Card>
                  ))}
                </Stack>
              )}
            </Stack>
          )}
        </Container>
      </AppShell.Main>
    </AppShell>
  )
}

export default App
