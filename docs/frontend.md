# Waypoint Frontend Documentation

## Overview
The Waypoint frontend is a React Single Page Application (SPA) that provides a responsive and intuitive interface for touring musicians to manage their network of hosts. The application enables viewing, creating, and managing hosts and stay records with an emphasis on geographical visualization.

## Project Structure

```
waypoint-client/
├── public/                # Static files
├── src/
│   ├── api/               # API service layer
│   ├── assets/            # Images, fonts, etc.
│   ├── components/        # Reusable React components
│   │   ├── common/        # Generic UI components
│   │   ├── hosts/         # Host-specific components
│   │   ├── stays/         # Stay-specific components
│   │   └── maps/          # Map and location components
│   ├── contexts/          # React contexts for state management
│   ├── hooks/             # Custom React hooks
│   ├── layouts/           # Page layout components
│   ├── pages/             # Page components
│   ├── routes/            # Routing configuration
│   ├── services/          # Non-API services (geolocation, auth, etc.)
│   ├── styles/            # Global styles, themes, and variables
│   ├── types/             # TypeScript type definitions
│   ├── utils/             # Utility functions
│   ├── App.tsx            # Root component
│   ├── index.tsx          # Entry point
│   └── config.ts          # Application configuration
└── package.json           # Dependencies and scripts
```

## Component Hierarchy

```
App
├── AuthProvider
│   └── Router
│       ├── Layout
│       │   ├── Navbar
│       │   ├── Sidebar
│       │   └── Pages
│       │       ├── Login/Register
│       │       ├── Dashboard
│       │       │   ├── StatsWidget
│       │       │   ├── RecentStaysWidget
│       │       │   └── NearbyHostsMap
│       │       ├── HostList
│       │       │   ├── HostFilters
│       │       │   ├── HostCard
│       │       │   └── Pagination
│       │       ├── HostDetail
│       │       │   ├── HostInfo
│       │       │   ├── StayHistory
│       │       │   └── LocationMap
│       │       ├── AddEditHost
│       │       │   ├── HostForm
│       │       │   └── AddressAutocomplete
│       │       ├── MapView
│       │       │   ├── MapComponent
│       │       │   └── HostMarkers
│       │       ├── StayHistory
│       │       │   ├── StayFilters
│       │       │   ├── StayTimeline
│       │       │   └── StayList
│       │       ├── AddEditStay
│       │       │   └── StayForm
│       │       └── UserSettings
│       │           └── ProfileForm
│       └── AuthGuard
└── ToastContainer
```

## State Management

The application uses a combination of React Context API and local component state to manage application state:

### Application-wide State

1. **AuthContext**
   - Manages user authentication state
   - Provides login, logout, and token refresh functionality
   - Exposes the current user information

2. **ToastContext**
   - Handles application notifications
   - Provides methods for showing success, error, and info messages

3. **ThemeContext**
   - Manages UI theme preferences (light/dark mode)
   - Stores user theme preference

### Feature-specific State

1. **HostsContext**
   - Manages hosts data and CRUD operations
   - Maintains filters and sorting preferences
   - Handles pagination state

2. **StaysContext**
   - Manages stays data and CRUD operations
   - Maintains filters and date ranges
   - Handles pagination state

### State Management Implementation

```typescript
// AuthContext example
import React, { createContext, useContext, useState, useEffect } from 'react';
import { User } from '../types';
import authService from '../services/authService';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (userData: RegisterData) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check if user is already logged in on app load
    const initAuth = async () => {
      try {
        const userData = await authService.getCurrentUser();
        setUser(userData);
      } catch (error) {
        // Handle error or unauthorized state
      } finally {
        setLoading(false);
      }
    };

    initAuth();
  }, []);

  const login = async (email: string, password: string) => {
    setLoading(true);
    try {
      const userData = await authService.login(email, password);
      setUser(userData);
    } finally {
      setLoading(false);
    }
  };

  // Other auth methods...

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        loading,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
```

## Routing

The application uses React Router for navigation. Routes are protected by an AuthGuard component that redirects unauthenticated users to the login page.

### Route Configuration

```typescript
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthGuard } from './components/auth/AuthGuard';
import Layout from './layouts/MainLayout';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import HostList from './pages/HostList';
import HostDetail from './pages/HostDetail';
import AddEditHost from './pages/AddEditHost';
import MapView from './pages/MapView';
import StayHistory from './pages/StayHistory';
import AddEditStay from './pages/AddEditStay';
import UserSettings from './pages/UserSettings';
import { useAuth } from './contexts/AuthContext';

const AppRoutes = () => {
  const { isAuthenticated } = useAuth();

  return (
    <BrowserRouter>
      <Routes>
        {/* Public routes */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
          
        {/* Protected routes */}
        <Route element={<AuthGuard><Layout /></AuthGuard>}>
          <Route path="/" element={<Dashboard />} />
          <Route path="/hosts" element={<HostList />} />
          <Route path="/hosts/:id" element={<HostDetail />} />
          <Route path="/hosts/add" element={<AddEditHost />} />
          <Route path="/hosts/:id/edit" element={<AddEditHost />} />
          <Route path="/map" element={<MapView />} />
          <Route path="/stays" element={<StayHistory />} />
          <Route path="/stays/add" element={<AddEditStay />} />
          <Route path="/stays/:id/edit" element={<AddEditStay />} />
          <Route path="/settings" element={<UserSettings />} />
        </Route>
          
        {/* Fallback route */}
        <Route path="*" element={<Navigate to={isAuthenticated ? '/' : '/login'} />} />
      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;
```

## API Integration

The frontend uses a service-based approach to communicate with the backend API. Each API endpoint has a corresponding service method.

### API Service Layer

```typescript
// src/api/apiClient.ts - Base API client
import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Add JWT token to requests
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Handle token expiration
apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      
      try {
        // Attempt to refresh token
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await axios.post(`${API_BASE_URL}/auth/refresh-token`, { refreshToken });
        
        if (response.data.token) {
          localStorage.setItem('token', response.data.token);
          localStorage.setItem('refreshToken', response.data.refreshToken);
          
          // Retry original request with new token
          originalRequest.headers.Authorization = `Bearer ${response.data.token}`;
          return apiClient(originalRequest);
        }
      } catch (refreshError) {
        // Refresh failed, redirect to login
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        window.location.href = '/login';
      }
    }
    
    return Promise.reject(error);
  }
);

export default apiClient;
```

### Host API Service

```typescript
// src/api/hostService.ts
import apiClient from './apiClient';
import { Host, CreateHostRequest, UpdateHostRequest, PaginatedResponse } from '../types';

const getHosts = async (page = 1, pageSize = 10, searchTerm = '') => {
  const response = await apiClient.get<PaginatedResponse<Host>>('/hosts', {
    params: { page, pageSize, searchTerm }
  });
  return response.data;
};

const getHostById = async (id: string) => {
  const response = await apiClient.get<Host>(`/hosts/${id}`);
  return response.data;
};

const createHost = async (hostData: CreateHostRequest) => {
  const response = await apiClient.post<Host>('/hosts', hostData);
  return response.data;
};

const updateHost = async (id: string, hostData: UpdateHostRequest) => {
  const response = await apiClient.put<Host>(`/hosts/${id}`, hostData);
  return response.data;
};

const deleteHost = async (id: string) => {
  await apiClient.delete(`/hosts/${id}`);
};

const getNearbyHosts = async (lat: number, lng: number, radius: number) => {
  const response = await apiClient.get<Host[]>('/hosts/nearby', {
    params: { lat, lng, radius }
  });
  return response.data;
};

export default {
  getHosts,
  getHostById,
  createHost,
  updateHost,
  deleteHost,
  getNearbyHosts
};
```

## Authentication Flow

### Login Flow

1. User enters credentials on the login form
2. Form submits to auth service, which calls the backend API
3. On successful login, JWT token and refresh token are stored in localStorage
4. User is redirected to the dashboard
5. ApiClient interceptors automatically add the token to future requests

### Registration Flow

1. User enters details on the registration form
2. Form submits to auth service, which calls the backend API
3. On successful registration, user is either:
   - Automatically logged in (same as login flow)
   - Redirected to the login page with a success message

### Logout Flow

1. User clicks logout
2. Auth service clears tokens from localStorage
3. User is redirected to the login page
4. AuthContext updates state to reflect unauthenticated status

## Key Features Implementation

### Map Integration

The application uses Mapbox GL JS for interactive maps:

```typescript
// src/components/maps/MapComponent.tsx
import React, { useEffect, useRef } from 'react';
import mapboxgl from 'mapbox-gl';
import 'mapbox-gl/dist/mapbox-gl.css';
import { Host } from '../../types';

interface MapComponentProps {
  hosts: Host[];
  center?: [number, number];
  zoom?: number;
  onMarkerClick?: (host: Host) => void;
}

const MapComponent: React.FC<MapComponentProps> = ({
  hosts,
  center = [-98.5795, 39.8283], // US center
  zoom = 4,
  onMarkerClick
}) => {
  const mapContainer = useRef<HTMLDivElement>(null);
  const map = useRef<mapboxgl.Map | null>(null);
  const markers = useRef<mapboxgl.Marker[]>([]);

  useEffect(() => {
    mapboxgl.accessToken = process.env.REACT_APP_MAPBOX_TOKEN || '';
    
    if (mapContainer.current && !map.current) {
      map.current = new mapboxgl.Map({
        container: mapContainer.current,
        style: 'mapbox://styles/mapbox/streets-v11',
        center,
        zoom
      });
      
      map.current.addControl(new mapboxgl.NavigationControl(), 'top-right');
    }
    
    return () => {
      map.current?.remove();
      map.current = null;
    };
  }, []);
  
  // Add markers for hosts
  useEffect(() => {
    if (!map.current) return;
    
    // Clear existing markers
    markers.current.forEach(marker => marker.remove());
    markers.current = [];
    
    // Add new markers
    hosts.forEach(host => {
      if (host.latitude && host.longitude) {
        const marker = new mapboxgl.Marker()
          .setLngLat([host.longitude, host.latitude])
          .addTo(map.current!);
        
        if (onMarkerClick) {
          marker.getElement().addEventListener('click', () => {
            onMarkerClick(host);
          });
        }
        
        markers.current.push(marker);
      }
    });
  }, [hosts, onMarkerClick]);
  
  return <div ref={mapContainer} style={{ width: '100%', height: '500px' }} />;
};

export default MapComponent;
```

### Address Geocoding

Integration with Mapbox Geocoding API for address autocompletion and coordinate lookup:

```typescript
// src/services/geocodingService.ts
import axios from 'axios';

const MAPBOX_API = 'https://api.mapbox.com/geocoding/v5/mapbox.places';
const ACCESS_TOKEN = process.env.REACT_APP_MAPBOX_TOKEN;

export interface GeocodingResult {
  id: string;
  place_name: string;
  center: [number, number]; // [longitude, latitude]
  properties: {
    accuracy?: string;
  };
  context: Array<{
    id: string;
    text: string;
  }>;
}

const searchAddress = async (query: string, limit = 5): Promise<GeocodingResult[]> => {
  if (!query) return [];
  
  const response = await axios.get(
    `${MAPBOX_API}/${encodeURIComponent(query)}.json`,
    {
      params: {
        access_token: ACCESS_TOKEN,
        country: 'us',
        types: 'address',
        limit
      }
    }
  );
  
  return response.data.features;
};

const getCoordinatesFromAddress = async (address: string): Promise<[number, number] | null> => {
  const results = await searchAddress(address, 1);
  
  if (results.length === 0) {
    return null;
  }
  
  return results[0].center;
};

export default {
  searchAddress,
  getCoordinatesFromAddress
};
```

## UI/UX Design Guidelines

### Design System

The application follows a cohesive design system:

1. **Color Palette**
   - Primary: `#3B82F6` (Blue)
   - Secondary: `#6366F1` (Indigo)
   - Accent: `#8B5CF6` (Purple)
   - Success: `#10B981` (Green)
   - Warning: `#F59E0B` (Amber)
   - Error: `#EF4444` (Red)
   - Background: `#F9FAFB` (Light Gray)
   - Text: `#1F2937` (Dark Gray)

2. **Typography**
   - Font family: 'Inter', sans-serif
   - Base size: 16px
   - Scale: 1.25 (major third)
   - Weights: 400 (regular), 500 (medium), 600 (semibold), 700 (bold)

3. **Spacing System**
   - Base unit: 4px
   - Scale: 4, 8, 12, 16, 24, 32, 48, 64, 96, 128

4. **Component Library**
   - Base: Material-UI or Chakra UI
   - Customized to match application design

### Responsive Design

The application is fully responsive with breakpoints:
- Small: 0-640px (mobile)
- Medium: 641-768px (tablet)
- Large: 769-1024px (small laptop)
- Extra Large: 1025-1280px (desktop)
- 2XL: 1281px+ (large screens)

### Accessibility

The application follows WCAG 2.1 AA standards:
- Semantic HTML
- Proper color contrast
- Keyboard navigation
- ARIA attributes where needed
- Screen reader support

## Performance Optimization

1. **Code Splitting**
   - Route-based code splitting with React.lazy and Suspense
   - Dynamic imports for large components

2. **Memoization**
   - React.memo for expensive components
   - useMemo for expensive calculations
   - useCallback for functions passed to child components

3. **Resource Loading**
   - Lazy loading of images
   - Preloading of critical resources
   - Optimized bundle size

4. **Rendering Optimization**
   - Virtualized lists for large datasets
   - Windowing techniques for map markers
   - Debounced inputs for search

## Testing Strategy

1. **Unit Tests**
   - Component rendering
   - Custom hooks
   - Utility functions
   - Context providers

2. **Integration Tests**
   - Form submissions
   - API service mocking
   - Authentication flows

3. **End-to-End Tests**
   - Critical user journeys
   - Cross-browser compatibility

## Deployment

The frontend application is deployed using Vercel:

1. **Environment Configuration**
   - Environment variables for API URLs
   - Feature flags
   - Production/development builds

2. **Build Process**
   - Optimized production builds
   - Static asset optimization
   - Bundle analysis

3. **Deployment Pipeline**
   - Continuous integration with GitHub Actions
   - Preview deployments for pull requests
   - Automated tests before deployment 