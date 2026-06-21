import { Genre } from '../types/Genre';
import { Artist } from '../types/Artist';
import { Track } from '../types/Track';
import { Playlist, PlaylistSummary } from '../types/Playlist';

const BASE_URL = 'http://localhost:5270/api';

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    throw new Error(errorData?.error || `Erro ${response.status}: ${response.statusText}`);
  }
  if (response.status === 204) return {} as T;
  return response.json();
}

// Genres
export const genreApi = {
  getAll: (): Promise<Genre[]> =>
    fetch(`${BASE_URL}/genres`).then(r => handleResponse<Genre[]>(r)),
  getById: (id: number): Promise<Genre> =>
    fetch(`${BASE_URL}/genres/${id}`).then(r => handleResponse<Genre>(r)),
  create: (genre: Omit<Genre, 'id'>): Promise<Genre> =>
    fetch(`${BASE_URL}/genres`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(genre)
    }).then(r => handleResponse<Genre>(r)),
  update: (id: number, genre: Omit<Genre, 'id'>): Promise<Genre> =>
    fetch(`${BASE_URL}/genres/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ ...genre, id })
    }).then(r => handleResponse<Genre>(r)),
  delete: (id: number): Promise<void> =>
    fetch(`${BASE_URL}/genres/${id}`, { method: 'DELETE' }).then(r => handleResponse<void>(r))
};

// Artists
export const artistApi = {
  getAll: (): Promise<Artist[]> =>
    fetch(`${BASE_URL}/artists`).then(r => handleResponse<Artist[]>(r)),
  getById: (id: number): Promise<Artist> =>
    fetch(`${BASE_URL}/artists/${id}`).then(r => handleResponse<Artist>(r)),
  create: (artist: Omit<Artist, 'id' | 'genreName'>): Promise<Artist> =>
    fetch(`${BASE_URL}/artists`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(artist)
    }).then(r => handleResponse<Artist>(r)),
  update: (id: number, artist: Omit<Artist, 'id' | 'genreName'>): Promise<Artist> =>
    fetch(`${BASE_URL}/artists/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ ...artist, id })
    }).then(r => handleResponse<Artist>(r)),
  delete: (id: number): Promise<void> =>
    fetch(`${BASE_URL}/artists/${id}`, { method: 'DELETE' }).then(r => handleResponse<void>(r))
};

// Tracks
export const trackApi = {
  getAll: (): Promise<Track[]> =>
    fetch(`${BASE_URL}/tracks`).then(r => handleResponse<Track[]>(r)),
  getById: (id: number): Promise<Track> =>
    fetch(`${BASE_URL}/tracks/${id}`).then(r => handleResponse<Track>(r)),
  create: (track: Omit<Track, 'id' | 'artistName'>): Promise<Track> =>
    fetch(`${BASE_URL}/tracks`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(track)
    }).then(r => handleResponse<Track>(r)),
  update: (id: number, track: Omit<Track, 'id' | 'artistName'>): Promise<Track> =>
    fetch(`${BASE_URL}/tracks/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ ...track, id })
    }).then(r => handleResponse<Track>(r)),
  delete: (id: number): Promise<void> =>
    fetch(`${BASE_URL}/tracks/${id}`, { method: 'DELETE' }).then(r => handleResponse<void>(r))
};

// Playlists
export const playlistApi = {
  getAll: (): Promise<Playlist[]> =>
    fetch(`${BASE_URL}/playlists`).then(r => handleResponse<Playlist[]>(r)),
  getById: (id: number): Promise<Playlist> =>
    fetch(`${BASE_URL}/playlists/${id}`).then(r => handleResponse<Playlist>(r)),
  create: (playlist: Omit<Playlist, 'id' | 'createdAt'>): Promise<Playlist> =>
    fetch(`${BASE_URL}/playlists`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(playlist)
    }).then(r => handleResponse<Playlist>(r)),
  update: (id: number, playlist: Omit<Playlist, 'id' | 'createdAt'>): Promise<Playlist> =>
    fetch(`${BASE_URL}/playlists/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ ...playlist, id })
    }).then(r => handleResponse<Playlist>(r)),
  delete: (id: number): Promise<void> =>
    fetch(`${BASE_URL}/playlists/${id}`, { method: 'DELETE' }).then(r => handleResponse<void>(r)),
  getTracks: (playlistId: number): Promise<Track[]> =>
    fetch(`${BASE_URL}/playlists/${playlistId}/tracks`).then(r => handleResponse<Track[]>(r)),
  addTrack: (playlistId: number, trackId: number): Promise<void> =>
    fetch(`${BASE_URL}/playlists/${playlistId}/tracks/${trackId}`, { method: 'POST' }).then(r => handleResponse<void>(r)),
  removeTrack: (playlistId: number, trackId: number): Promise<void> =>
    fetch(`${BASE_URL}/playlists/${playlistId}/tracks/${trackId}`, { method: 'DELETE' }).then(r => handleResponse<void>(r)),
  getSummary: (playlistId: number): Promise<PlaylistSummary> =>
    fetch(`${BASE_URL}/playlists/${playlistId}/summary`).then(r => handleResponse<PlaylistSummary>(r))
};
