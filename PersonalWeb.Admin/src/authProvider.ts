import { AuthProvider } from 'react-admin';

const AUTH_TOKEN_KEY = 'auth_token';

const authProvider: AuthProvider = {
  login: async ({ username, password }) => {
    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email: username, password }),
      credentials: 'include',
    });

    if (!response.ok) {
      throw new Error('Invalid email or password.');
    }

    const { accessToken } = await response.json();
    localStorage.setItem(AUTH_TOKEN_KEY, accessToken);
  },

  logout: async () => {
    localStorage.removeItem(AUTH_TOKEN_KEY);
  },

  checkAuth: async () => {
    if (!localStorage.getItem(AUTH_TOKEN_KEY)) {
      throw new Error('Not authenticated');
    }
  },

  checkError: async (error) => {
    if (error?.status === 401 || error?.status === 403) {
      localStorage.removeItem(AUTH_TOKEN_KEY);
      throw new Error('Session expired');
    }
  },

  getPermissions: async () => {
    const token = localStorage.getItem(AUTH_TOKEN_KEY);
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? payload.role ?? null;
    } catch {
      return null;
    }
  },

  getIdentity: async () => {
    const token = localStorage.getItem(AUTH_TOKEN_KEY);
    if (!token) throw new Error('Not authenticated');
    const payload = JSON.parse(atob(token.split('.')[1]));
    return { id: payload.sub, fullName: payload.email };
  },
};

export const getAuthToken = (): string | null => localStorage.getItem(AUTH_TOKEN_KEY);

export default authProvider;
